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
        if (updateDrinkDto.StockQuantity.HasValue) drink.StockQuantity = updateDrinkDto.StockQuantity.Value;
        if (updateDrinkDto.ImageUrl != null) drink.ImageUrl = updateDrinkDto.ImageUrl;
        if (updateDrinkDto.CategoryId.HasValue) drink.CategoryId = updateDrinkDto.CategoryId.Value;
        if (updateDrinkDto.IsAvailable.HasValue) drink.IsAvailable = updateDrinkDto.IsAvailable.Value;

        drink.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

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


