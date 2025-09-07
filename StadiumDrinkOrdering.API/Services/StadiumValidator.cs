using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Services;

public static class StadiumValidator
{
    public static ValidationResult Validate(StadiumImportDto stadium)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrEmpty(stadium.Name))
            errors.Add("Stadium name is required");
        
        if (stadium.Tribunes == null || !stadium.Tribunes.Any())
            errors.Add("At least one tribune is required");
        
        var validCodes = new[] { "N", "S", "E", "W" };
        var usedCodes = new HashSet<string>();
        
        foreach (var tribune in stadium.Tribunes ?? new List<TribuneImportDto>())
        {
            if (string.IsNullOrEmpty(tribune.Code) || tribune.Code.Length != 1)
                errors.Add($"Tribune '{tribune.Name}' must have a single character code");
            else if (!validCodes.Contains(tribune.Code))
                errors.Add($"Tribune '{tribune.Name}' has invalid code '{tribune.Code}'. Valid codes: N, S, E, W");
            else if (!usedCodes.Add(tribune.Code))
                errors.Add($"Duplicate tribune code '{tribune.Code}'");
            
            if (tribune.Rings == null || !tribune.Rings.Any())
                errors.Add($"Tribune '{tribune.Name}' must have at least one ring");
            
            var ringNumbers = new HashSet<int>();
            foreach (var ring in tribune.Rings ?? new List<RingImportDto>())
            {
                if (ring.Number < 1)
                    errors.Add($"Ring number must be positive in tribune '{tribune.Name}'");
                else if (!ringNumbers.Add(ring.Number))
                    errors.Add($"Duplicate ring number {ring.Number} in tribune '{tribune.Name}'");
                
                if (ring.Sectors == null || !ring.Sectors.Any())
                    errors.Add($"Ring {ring.Number} in tribune '{tribune.Name}' must have at least one sector");
                
                var sectorCodes = new HashSet<string>();
                foreach (var sector in ring.Sectors ?? new List<SectorImportDto>())
                {
                    if (string.IsNullOrEmpty(sector.Code))
                        errors.Add($"Sector must have a code in ring {ring.Number} of tribune '{tribune.Name}'");
                    else if (!sectorCodes.Add(sector.Code))
                        errors.Add($"Duplicate sector code '{sector.Code}' in ring {ring.Number} of tribune '{tribune.Name}'");
                    
                    if (sector.Rows <= 0)
                        errors.Add($"Sector '{sector.Code}' must have positive number of rows");
                    
                    if (sector.SeatsPerRow <= 0)
                        errors.Add($"Sector '{sector.Code}' must have positive number of seats per row");
                }
            }
        }
        
        return new ValidationResult 
        { 
            IsValid = errors.Count == 0, 
            Errors = errors 
        };
    }
}