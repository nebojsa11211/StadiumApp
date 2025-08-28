using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

[Route("api/[controller]")]
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
            .Where(d => d.IsAvailable)
            .OrderBy(d => d.Category)
            .ThenBy(d => d.Name)
            .ToListAsync();

        var drinkDtos = drinks.Select(d => new DrinkDto
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            Price = d.Price,
            StockQuantity = d.StockQuantity,
            ImageUrl = d.ImageUrl,
            Category = d.Category,
            IsAvailable = d.IsAvailable
        }).ToList();

        return Ok(drinkDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DrinkDto>> GetDrink(int id)
    {
        var drink = await _context.Drinks.FindAsync(id);
        if (drink == null)
        {
            return NotFound();
        }

        var drinkDto = new DrinkDto
        {
            Id = drink.Id,
            Name = drink.Name,
            Description = drink.Description,
            Price = drink.Price,
            StockQuantity = drink.StockQuantity,
            ImageUrl = drink.ImageUrl,
            Category = drink.Category,
            IsAvailable = drink.IsAvailable
        };

        return Ok(drinkDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DrinkDto>> CreateDrink([FromBody] CreateDrinkDto createDrinkDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var drink = new Drink
        {
            Name = createDrinkDto.Name,
            Description = createDrinkDto.Description,
            Price = createDrinkDto.Price,
            StockQuantity = createDrinkDto.StockQuantity,
            ImageUrl = createDrinkDto.ImageUrl,
            Category = createDrinkDto.Category,
            IsAvailable = createDrinkDto.IsAvailable,
            CreatedAt = DateTime.UtcNow
        };

        _context.Drinks.Add(drink);
        await _context.SaveChangesAsync();

        var drinkDto = new DrinkDto
        {
            Id = drink.Id,
            Name = drink.Name,
            Description = drink.Description,
            Price = drink.Price,
            StockQuantity = drink.StockQuantity,
            ImageUrl = drink.ImageUrl,
            Category = drink.Category,
            IsAvailable = drink.IsAvailable
        };

        return CreatedAtAction(nameof(GetDrink), new { id = drink.Id }, drinkDto);
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

        if (updateDrinkDto.Name != null) drink.Name = updateDrinkDto.Name;
        if (updateDrinkDto.Description != null) drink.Description = updateDrinkDto.Description;
        if (updateDrinkDto.Price.HasValue) drink.Price = updateDrinkDto.Price.Value;
        if (updateDrinkDto.StockQuantity.HasValue) drink.StockQuantity = updateDrinkDto.StockQuantity.Value;
        if (updateDrinkDto.ImageUrl != null) drink.ImageUrl = updateDrinkDto.ImageUrl;
        if (updateDrinkDto.Category.HasValue) drink.Category = updateDrinkDto.Category.Value;
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
}


