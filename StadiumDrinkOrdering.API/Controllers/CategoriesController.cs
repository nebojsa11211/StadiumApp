using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var categories = await _context.Categories
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                DisplayName = c.DisplayName,
                Icon = c.Icon,
                IsActive = c.IsActive,
                SortOrder = c.SortOrder,
                DrinkCount = c.Drinks.Count
            })
            .ToListAsync();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var category = await _context.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                DisplayName = c.DisplayName,
                Icon = c.Icon,
                IsActive = c.IsActive,
                SortOrder = c.SortOrder,
                DrinkCount = c.Drinks.Count
            })
            .FirstOrDefaultAsync();

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var name = dto.Name.Trim();
        if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower()))
            return Conflict($"A category named '{name}' already exists.");

        var category = new Category
        {
            Name = name,
            DisplayName = string.IsNullOrWhiteSpace(dto.DisplayName) ? null : dto.DisplayName.Trim(),
            Icon = string.IsNullOrWhiteSpace(dto.Icon) ? null : dto.Icon.Trim(),
            IsActive = dto.IsActive,
            SortOrder = dto.SortOrder,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, ToDto(category, 0));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        if (dto.Name != null)
        {
            var name = dto.Name.Trim();
            if (string.IsNullOrEmpty(name))
                return BadRequest("Category name cannot be empty.");
            if (await _context.Categories.AnyAsync(c => c.Id != id && c.Name.ToLower() == name.ToLower()))
                return Conflict($"A category named '{name}' already exists.");
            category.Name = name;
        }

        if (dto.DisplayName != null)
            category.DisplayName = string.IsNullOrWhiteSpace(dto.DisplayName) ? null : dto.DisplayName.Trim();
        if (dto.Icon != null)
            category.Icon = string.IsNullOrWhiteSpace(dto.Icon) ? null : dto.Icon.Trim();
        if (dto.IsActive.HasValue)
            category.IsActive = dto.IsActive.Value;
        if (dto.SortOrder.HasValue)
            category.SortOrder = dto.SortOrder.Value;

        category.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var drinkCount = await _context.Drinks.CountAsync(d => d.CategoryId == id);
        return Ok(ToDto(category, drinkCount));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        var drinkCount = await _context.Drinks.CountAsync(d => d.CategoryId == id);
        if (drinkCount > 0)
        {
            return Conflict(
                $"Cannot delete category '{category.Name}' because {drinkCount} drink(s) still use it. " +
                "Reassign those drinks to another category first.");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static CategoryDto ToDto(Category c, int drinkCount) => new()
    {
        Id = c.Id,
        Name = c.Name,
        DisplayName = c.DisplayName,
        Icon = c.Icon,
        IsActive = c.IsActive,
        SortOrder = c.SortOrder,
        DrinkCount = drinkCount
    };
}
