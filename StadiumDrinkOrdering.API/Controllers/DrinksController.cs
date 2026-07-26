using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

[Route("[controller]")]
[ApiController]
public class DrinksController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DrinksController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<DrinkDto>>> GetDrinks()
    {
        var drinks = await _context.Drinks
            .Include(d => d.Category)
            .Where(d => d.IsAvailable)
            .OrderBy(d => d.Category!.SortOrder)
            .ThenBy(d => d.Name)
            .ToListAsync();

        var drinkDtos = drinks.Select(MapToDto).ToList();

        return Ok(drinkDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DrinkDto>> GetDrink(int id)
    {
        var drink = await _context.Drinks
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (drink == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(drink));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DrinkDto>> CreateDrink([FromBody] CreateDrinkDto createDrinkDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!await _context.Categories.AnyAsync(c => c.Id == createDrinkDto.CategoryId))
        {
            return BadRequest("The selected category does not exist.");
        }

        var drink = new Drink
        {
            Name = createDrinkDto.Name,
            Description = createDrinkDto.Description,
            Price = createDrinkDto.Price,
            StockQuantity = createDrinkDto.StockQuantity,
            ImageUrl = createDrinkDto.ImageUrl,
            CategoryId = createDrinkDto.CategoryId,
            IsAvailable = createDrinkDto.IsAvailable,
            CreatedAt = DateTime.UtcNow
        };

        _context.Drinks.Add(drink);
        await _context.SaveChangesAsync();
        await _context.Entry(drink).Reference(d => d.Category).LoadAsync();

        return CreatedAtAction(nameof(GetDrink), new { id = drink.Id }, MapToDto(drink));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDrink(int id, [FromBody] UpdateDrinkDto updateDrinkDto)
    {
        var drink = await _context.Drinks.FindAsync(id);
        if (drink == null)
        {
            return NotFound();
        }

        if (updateDrinkDto.CategoryId.HasValue &&
            !await _context.Categories.AnyAsync(c => c.Id == updateDrinkDto.CategoryId.Value))
        {
            return BadRequest("The selected category does not exist.");
        }

        if (updateDrinkDto.Name != null) drink.Name = updateDrinkDto.Name;
        if (updateDrinkDto.Description != null) drink.Description = updateDrinkDto.Description;
        if (updateDrinkDto.Price.HasValue) drink.Price = updateDrinkDto.Price.Value;

        // Editing the raw stock number is an absolute set. Record the resulting delta as a
        // ManualAdjustment so corrections are auditable alongside restocks and sales. The ledger row
        // is added to the same SaveChanges as the drink update, so the two commit atomically.
        if (updateDrinkDto.StockQuantity.HasValue && updateDrinkDto.StockQuantity.Value != drink.StockQuantity)
        {
            var delta = updateDrinkDto.StockQuantity.Value - drink.StockQuantity;
            drink.StockQuantity = updateDrinkDto.StockQuantity.Value;
            _context.StockMovements.Add(new StockMovement
            {
                DrinkId = drink.Id,
                Delta = delta,
                QuantityAfter = drink.StockQuantity,
                Type = StockMovementType.ManualAdjustment,
                UserId = CurrentUserId(),
                UserEmail = CurrentUserEmail(),
                CreatedAt = DateTime.UtcNow
            });
        }

        if (updateDrinkDto.ImageUrl != null) drink.ImageUrl = updateDrinkDto.ImageUrl;
        if (updateDrinkDto.CategoryId.HasValue) drink.CategoryId = updateDrinkDto.CategoryId.Value;
        if (updateDrinkDto.IsAvailable.HasValue) drink.IsAvailable = updateDrinkDto.IsAvailable.Value;

        drink.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Adds a delta amount of stock to a drink and records a Restock ledger entry attributed to the
    /// current admin. This is the concurrency-safer alternative to overwriting the absolute stock number:
    /// the increment and the ledger row commit in one SaveChanges, and the recorded QuantityAfter is the
    /// balance produced by this operation.
    /// </summary>
    [HttpPost("{id}/restock")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DrinkDto>> RestockDrink(int id, [FromBody] RestockDrinkDto restockDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var drink = await _context.Drinks
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (drink == null)
        {
            return NotFound();
        }

        drink.StockQuantity += restockDto.Quantity;
        drink.UpdatedAt = DateTime.UtcNow;

        _context.StockMovements.Add(new StockMovement
        {
            DrinkId = drink.Id,
            Delta = restockDto.Quantity,
            QuantityAfter = drink.StockQuantity,
            Type = StockMovementType.Restock,
            Note = string.IsNullOrWhiteSpace(restockDto.Note) ? null : restockDto.Note.Trim(),
            UserId = CurrentUserId(),
            UserEmail = CurrentUserEmail(),
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return Ok(MapToDto(drink));
    }

    /// <summary>
    /// Returns a drink's most recent stock movements (newest first) for the admin history view.
    /// </summary>
    [HttpGet("{id}/stock-movements")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<StockMovementDto>>> GetStockMovements(int id, [FromQuery] int take = 50)
    {
        if (!await _context.Drinks.AnyAsync(d => d.Id == id))
        {
            return NotFound();
        }

        take = Math.Clamp(take, 1, 200);

        var movements = await _context.StockMovements
            .Where(m => m.DrinkId == id)
            .OrderByDescending(m => m.CreatedAt)
            .ThenByDescending(m => m.Id)
            .Take(take)
            .Select(m => new StockMovementDto
            {
                Id = m.Id,
                DrinkId = m.DrinkId,
                Delta = m.Delta,
                QuantityAfter = m.QuantityAfter,
                Type = m.Type.ToString(),
                Note = m.Note,
                UserEmail = m.UserEmail,
                OrderId = m.OrderId,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync();

        return Ok(movements);
    }

    /// <summary>
    /// Generates the common starter catalog (standard categories + drinks) for a venue that has no
    /// assortment yet. Idempotent: anything already present — matched on a loose name key, so
    /// "Coca-Cola" won't be added next to an existing "Coca Cola" — is left untouched and reported
    /// as skipped, which makes the button safe to press more than once.
    /// </summary>
    [HttpPost("seed-catalog")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SeedCatalogResultDto>> SeedCatalog()
    {
        var result = new SeedCatalogResultDto();
        var now = DateTime.UtcNow;

        var existingCategories = await _context.Categories.ToListAsync();
        var categoriesByKey = existingCategories
            .GroupBy(c => DefaultDrinkCatalog.NameKey(c.Name))
            .ToDictionary(g => g.Key, g => g.First());

        foreach (var template in DefaultDrinkCatalog.Categories)
        {
            var key = DefaultDrinkCatalog.NameKey(template.Name);
            if (categoriesByKey.ContainsKey(key))
            {
                result.CategoriesSkipped++;
                continue;
            }

            var category = new Category
            {
                Name = template.Name,
                DisplayName = template.DisplayName,
                Icon = template.Icon,
                IsActive = true,
                SortOrder = template.SortOrder,
                CreatedAt = now
            };

            _context.Categories.Add(category);
            categoriesByKey[key] = category;
            result.CategoriesCreated++;
        }

        // Save first so the newly added categories get their ids before drinks reference them.
        if (result.CategoriesCreated > 0)
        {
            await _context.SaveChangesAsync();
        }

        var existingDrinkKeys = (await _context.Drinks.Select(d => d.Name).ToListAsync())
            .Select(DefaultDrinkCatalog.NameKey)
            .ToHashSet();

        foreach (var template in DefaultDrinkCatalog.Drinks)
        {
            var drinkKey = DefaultDrinkCatalog.NameKey(template.Name);
            if (!existingDrinkKeys.Add(drinkKey))
            {
                result.DrinksSkipped++;
                continue;
            }

            if (!categoriesByKey.TryGetValue(DefaultDrinkCatalog.NameKey(template.CategoryName), out var category))
            {
                // Should not happen (every catalog category is created above), but never orphan a drink.
                result.DrinksSkipped++;
                continue;
            }

            _context.Drinks.Add(new Drink
            {
                Name = template.Name,
                Description = template.Description,
                Price = template.Price,
                StockQuantity = template.StockQuantity,
                CategoryId = category.Id,
                IsAvailable = true,
                CreatedAt = now
            });

            result.DrinksCreated++;
            result.CreatedDrinkNames.Add(template.Name);
        }

        if (result.DrinksCreated > 0)
        {
            await _context.SaveChangesAsync();
        }

        return Ok(result);
    }

    private int? CurrentUserId() =>
        int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

    private string? CurrentUserEmail() => User.FindFirst(ClaimTypes.Email)?.Value;

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDrink(int id)
    {
        var drink = await _context.Drinks.FindAsync(id);
        if (drink == null)
        {
            return NotFound();
        }

        drink.IsAvailable = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static DrinkDto MapToDto(Drink d) => new()
    {
        Id = d.Id,
        Name = d.Name,
        Description = d.Description,
        Price = d.Price,
        StockQuantity = d.StockQuantity,
        ImageUrl = d.ImageUrl,
        CategoryId = d.CategoryId,
        CategoryName = d.Category?.Name,
        CategoryDisplayName = d.Category?.DisplayName,
        CategoryIcon = d.Category?.Icon,
        IsAvailable = d.IsAvailable
    };
}


