using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public class StadiumStructureService : IStadiumStructureService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StadiumStructureService> _logger;

    public StadiumStructureService(ApplicationDbContext context, ILogger<StadiumStructureService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> ImportFromJsonAsync(IFormFile jsonFile)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // 1. Clear existing structure
            await ClearExistingStructureAsync();
            
            // 2. Read and parse JSON
            using var streamReader = new StreamReader(jsonFile.OpenReadStream());
            var jsonContent = await streamReader.ReadToEndAsync();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var stadiumData = JsonSerializer.Deserialize<StadiumImportDto>(jsonContent, jsonOptions);
            
            if (stadiumData?.Tribunes == null)
                throw new ArgumentException("Invalid JSON structure");
            
            // 3. Validate data
            var validationResult = StadiumValidator.Validate(stadiumData);
            if (!validationResult.IsValid)
                throw new ArgumentException($"Validation failed: {string.Join(", ", validationResult.Errors)}");
            
            // 4. Save new structure
            foreach (var tribuneDto in stadiumData.Tribunes)
            {
                var tribune = new Tribune 
                { 
                    Code = tribuneDto.Code, 
                    Name = tribuneDto.Name,
                    Description = tribuneDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                _context.Tribunes.Add(tribune);
                await _context.SaveChangesAsync();
                
                foreach (var ringDto in tribuneDto.Rings)
                {
                    var ring = new Ring 
                    { 
                        TribuneId = tribune.Id, 
                        Number = ringDto.Number, 
                        Name = ringDto.Name,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    _context.Rings.Add(ring);
                    await _context.SaveChangesAsync();
                    
                    foreach (var sectorDto in ringDto.Sectors)
                    {
                        var sector = new Sector 
                        { 
                            RingId = ring.Id,
                            Code = sectorDto.Code,
                            Name = sectorDto.Name,
                            TotalRows = sectorDto.Rows,
                            SeatsPerRow = sectorDto.SeatsPerRow,
                            StartRow = sectorDto.StartRow,
                            StartSeat = sectorDto.StartSeat,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        
                        _context.Sectors.Add(sector);
                        await _context.SaveChangesAsync();
                        
                        // Generate seats for sector
                        await GenerateSeatsForSectorAsync(sector.Id);
                    }
                }
            }
            
            await transaction.CommitAsync();
            _logger.LogInformation("Stadium structure imported successfully from JSON");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to import stadium structure from JSON");
            return false;
        }
    }

    public async Task<bool> ImportFromCsvAsync(IFormFile csvFile)
    {
        // TODO: Implement CSV import if needed
        _logger.LogWarning("CSV import not yet implemented");
        await Task.CompletedTask;
        return false;
    }

    public async Task<bool> GenerateSeatsForSectorAsync(int sectorId)
    {
        var sector = await _context.Sectors
            .Include(s => s.Ring)
            .ThenInclude(r => r.Tribune)
            .FirstOrDefaultAsync(s => s.Id == sectorId);
        
        if (sector == null) return false;
        
        var seats = new List<StadiumSeatNew>();
        
        for (int row = sector.StartRow; row < sector.StartRow + sector.TotalRows; row++)
        {
            for (int seatNum = sector.StartSeat; seatNum < sector.StartSeat + sector.SeatsPerRow; seatNum++)
            {
                var uniqueCode = $"{sector.Ring!.Tribune!.Code}-{sector.Ring.Number}-{sector.Code}-{row}-{seatNum}";
                
                seats.Add(new StadiumSeatNew 
                { 
                    SectorId = sector.Id,
                    RowNumber = row,
                    SeatNumber = seatNum,
                    UniqueCode = uniqueCode,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }
        
        _context.StadiumSeatsNew.AddRange(seats);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> ClearExistingStructureAsync()
    {
        try
        {
            // Delete in order due to foreign key constraints
            await _context.StadiumSeatsNew.ExecuteDeleteAsync();
            await _context.Sectors.ExecuteDeleteAsync();
            await _context.Rings.ExecuteDeleteAsync();
            await _context.Tribunes.ExecuteDeleteAsync();
            
            _logger.LogInformation("Existing stadium structure cleared");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to clear existing stadium structure");
            return false;
        }
    }

    public async Task<StadiumSummaryDto> GetStadiumSummaryAsync()
    {
        var summary = new StadiumSummaryDto
        {
            TotalTribunes = await _context.Tribunes.CountAsync(),
            TotalRings = await _context.Rings.CountAsync(),
            TotalSectors = await _context.Sectors.CountAsync(),
            TotalSeats = await _context.StadiumSeatsNew.CountAsync(),
            AvailableSeats = await _context.StadiumSeatsNew.CountAsync(s => s.IsAvailable),
            OccupiedSeats = await _context.StadiumSeatsNew.CountAsync(s => !s.IsAvailable)
        };
        
        return summary;
    }

    public async Task<List<Tribune>> GetFullStructureAsync()
    {
        return await _context.Tribunes
            .Include(t => t.Rings)
            .ThenInclude(r => r.Sectors)
            .ThenInclude(s => s.Seats)
            .OrderBy(t => t.Code)
            .ToListAsync();
    }

    public async Task<int> GetTotalSeatsCountAsync()
    {
        return await _context.StadiumSeatsNew.CountAsync();
    }

    public async Task<MemoryStream> ExportToJsonAsync()
    {
        var tribunes = await GetFullStructureAsync();
        
        var stadiumData = new StadiumImportDto
        {
            Name = "Stadium Structure Export",
            Description = $"Exported on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC",
            Tribunes = tribunes.Select(t => new TribuneImportDto
            {
                Code = t.Code,
                Name = t.Name,
                Description = t.Description ?? string.Empty,
                Rings = t.Rings.Select(r => new RingImportDto
                {
                    Number = r.Number,
                    Name = r.Name,
                    Sectors = r.Sectors.Select(s => new SectorImportDto
                    {
                        Code = s.Code,
                        Name = s.Name,
                        Rows = s.TotalRows,
                        SeatsPerRow = s.SeatsPerRow,
                        StartRow = s.StartRow,
                        StartSeat = s.StartSeat
                    }).ToList()
                }).ToList()
            }).ToList()
        };

        var json = JsonSerializer.Serialize(stadiumData, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(json);
        await writer.FlushAsync();
        stream.Position = 0;
        
        return stream;
    }

    public async Task<MemoryStream> ExportToCsvAsync()
    {
        // TODO: Implement CSV export if needed
        _logger.LogWarning("CSV export not yet implemented");
        await Task.CompletedTask;
        return new MemoryStream();
    }
}