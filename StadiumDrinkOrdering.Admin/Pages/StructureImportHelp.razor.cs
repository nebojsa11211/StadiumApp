using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class StructureImportHelp : ComponentBase
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private string jsonInput = "";
    private string validationResult = "";
    private bool isValidJson = false;
    private StadiumStructureDto? parsedStadium;

    private async Task CopyJsonStructure()
    {
        var jsonTemplate = @"{
  ""name"": ""My Stadium"",
  ""description"": ""Professional stadium venue"",
  ""tribunes"": [
    {
      ""code"": ""N"",
      ""name"": ""North Tribune"",
      ""description"": ""Main stand"",
      ""rings"": [
        {
          ""number"": 1,
          ""name"": ""Lower Ring"",
          ""sectors"": [
            {
              ""code"": ""NA"",
              ""name"": ""Sector A"",
              ""type"": ""standard"",
              ""rows"": 25,
              ""seatsPerRow"": 20,
              ""startRow"": 1,
              ""startSeat"": 1,
              ""priceCategory"": ""A""
            }
          ]
        }
      ]
    }
  ],
  ""metadata"": {
    ""version"": ""1.0"",
    ""createdDate"": ""2024-01-01""
  }
}";

        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", jsonTemplate);
        // Could add a toast notification here
    }

    private void ValidateJson()
    {
        if (string.IsNullOrWhiteSpace(jsonInput))
        {
            validationResult = "Please enter JSON content to validate.";
            isValidJson = false;
            return;
        }

        try
        {
            parsedStadium = JsonSerializer.Deserialize<StadiumStructureDto>(jsonInput, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (parsedStadium == null)
            {
                validationResult = "❌ Invalid JSON: Unable to parse stadium structure.";
                isValidJson = false;
                return;
            }

            // Perform validation
            var errors = new List<string>();

            // Stadium name validation
            if (string.IsNullOrWhiteSpace(parsedStadium.Name))
                errors.Add("Stadium name is required");
            else if (parsedStadium.Name.Length < 3 || parsedStadium.Name.Length > 100)
                errors.Add("Stadium name must be between 3 and 100 characters");

            // Tribunes validation
            if (!parsedStadium.Tribunes?.Any() == true)
            {
                errors.Add("At least one tribune is required");
            }
            else
            {
                var validTribuneCodes = new[] { "N", "S", "E", "W" };
                var tribuneCodes = new HashSet<string>();

                foreach (var tribune in parsedStadium.Tribunes)
                {
                    if (string.IsNullOrWhiteSpace(tribune.Code))
                        errors.Add("Tribune code is required");
                    else if (!validTribuneCodes.Contains(tribune.Code.ToUpper()))
                        errors.Add($"Invalid tribune code '{tribune.Code}'. Use N, S, E, or W only");
                    else if (!tribuneCodes.Add(tribune.Code.ToUpper()))
                        errors.Add($"Duplicate tribune code '{tribune.Code}'");

                    if (string.IsNullOrWhiteSpace(tribune.Name))
                        errors.Add("Tribune name is required");

                    if (!tribune.Rings?.Any() == true)
                        errors.Add($"Tribune '{tribune.Code}' must have at least one ring");
                    else
                    {
                        foreach (var ring in tribune.Rings)
                        {
                            if (ring.Number < 1 || ring.Number > 5)
                                errors.Add($"Ring number must be between 1 and 5 (Tribune {tribune.Code})");

                            if (string.IsNullOrWhiteSpace(ring.Name))
                                errors.Add($"Ring name is required (Tribune {tribune.Code}, Ring {ring.Number})");

                            if (!ring.Sectors?.Any() == true)
                                errors.Add($"Ring {ring.Number} in tribune '{tribune.Code}' must have at least one sector");
                            else
                            {
                                var sectorCodes = new HashSet<string>();
                                foreach (var sector in ring.Sectors)
                                {
                                    if (string.IsNullOrWhiteSpace(sector.Code))
                                        errors.Add("Sector code is required");
                                    else if (!sectorCodes.Add(sector.Code.ToUpper()))
                                        errors.Add($"Duplicate sector code '{sector.Code}'");

                                    if (string.IsNullOrWhiteSpace(sector.Name))
                                        errors.Add("Sector name is required");

                                    if (sector.Rows < 1 || sector.Rows > 100)
                                        errors.Add($"Sector rows must be between 1 and 100 (Sector {sector.Code})");

                                    if (sector.SeatsPerRow < 1 || sector.SeatsPerRow > 100)
                                        errors.Add($"Seats per row must be between 1 and 100 (Sector {sector.Code})");
                                }
                            }
                        }
                    }
                }
            }

            if (errors.Any())
            {
                validationResult = "❌ Validation failed:\n" + string.Join("\n", errors.Select(e => "• " + e));
                isValidJson = false;
            }
            else
            {
                var totalSeats = parsedStadium.Tribunes
                    .SelectMany(t => t.Rings)
                    .SelectMany(r => r.Sectors)
                    .Sum(s => s.Rows * s.SeatsPerRow);

                validationResult = $"✅ Valid JSON structure!\n" +
                                 $"• Stadium: {parsedStadium.Name}\n" +
                                 $"• Tribunes: {parsedStadium.Tribunes.Count}\n" +
                                 $"• Total Rings: {parsedStadium.Tribunes.Sum(t => t.Rings.Count)}\n" +
                                 $"• Total Sectors: {parsedStadium.Tribunes.SelectMany(t => t.Rings).Sum(r => r.Sectors.Count)}\n" +
                                 $"• Total Seats: {totalSeats:N0}";
                isValidJson = true;
            }
        }
        catch (JsonException ex)
        {
            validationResult = $"❌ JSON parsing error: {ex.Message}";
            isValidJson = false;
        }
        catch (Exception ex)
        {
            validationResult = $"❌ Validation error: {ex.Message}";
            isValidJson = false;
        }
    }

    private async Task DownloadSample(string sampleType)
    {
        var jsonContent = GetSampleJson(sampleType);
        var fileName = sampleType switch
        {
            "minimal" => "minimal-stadium.json",
            "standard" => "standard-stadium.json",
            "complex" => "complex-stadium.json",
            _ => "sample-stadium.json"
        };

        await JSRuntime.InvokeVoidAsync("downloadFile", fileName, jsonContent);
    }

    private void ClearValidator()
    {
        jsonInput = "";
        validationResult = "";
        isValidJson = false;
        parsedStadium = null;
    }

    private int CalculateTotalSeats(StadiumStructureDto stadium)
    {
        return stadium.Tribunes?
            .SelectMany(t => t.Rings)
            .SelectMany(r => r.Sectors)
            .Sum(s => s.Rows * s.SeatsPerRow) ?? 0;
    }

    private string GetSampleJson(string sampleType) => sampleType switch
    {
        "minimal" => @"{
  ""name"": ""Minimal Stadium"",
  ""description"": ""Basic stadium structure for testing"",
  ""tribunes"": [
    {
      ""code"": ""N"",
      ""name"": ""North Tribune"",
      ""rings"": [
        {
          ""number"": 1,
          ""name"": ""Main Stand"",
          ""sectors"": [
            {
              ""code"": ""NA"",
              ""name"": ""North A"",
              ""rows"": 10,
              ""seatsPerRow"": 20
            }
          ]
        }
      ]
    }
  ]
}",
        "standard" => @"{
  ""name"": ""Standard Stadium"",
  ""description"": ""Four-tribune professional stadium"",
  ""capacity"": 20000,
  ""tribunes"": [
    {
      ""code"": ""N"",
      ""name"": ""North Tribune"",
      ""rings"": [
        {
          ""number"": 1,
          ""name"": ""Lower Ring"",
          ""sectors"": [
            { ""code"": ""NA"", ""name"": ""North A"", ""rows"": 25, ""seatsPerRow"": 20 },
            { ""code"": ""NB"", ""name"": ""North B"", ""rows"": 25, ""seatsPerRow"": 20 }
          ]
        }
      ]
    },
    {
      ""code"": ""S"",
      ""name"": ""South Tribune"",
      ""rings"": [
        {
          ""number"": 1,
          ""name"": ""Lower Ring"",
          ""sectors"": [
            { ""code"": ""SA"", ""name"": ""South A"", ""rows"": 25, ""seatsPerRow"": 20 },
            { ""code"": ""SB"", ""name"": ""South B"", ""rows"": 25, ""seatsPerRow"": 20 }
          ]
        }
      ]
    },
    {
      ""code"": ""E"",
      ""name"": ""East Tribune"",
      ""rings"": [
        {
          ""number"": 1,
          ""name"": ""Lower Ring"",
          ""sectors"": [
            { ""code"": ""EA"", ""name"": ""East A"", ""rows"": 20, ""seatsPerRow"": 25 }
          ]
        }
      ]
    },
    {
      ""code"": ""W"",
      ""name"": ""West Tribune"",
      ""rings"": [
        {
          ""number"": 1,
          ""name"": ""Lower Ring"",
          ""sectors"": [
            { ""code"": ""WA"", ""name"": ""West A"", ""rows"": 20, ""seatsPerRow"": 25 }
          ]
        }
      ]
    }
  ]
}",
        "complex" => @"{
  ""name"": ""Complex Stadium"",
  ""description"": ""Large multi-ring stadium with VIP sections"",
  ""capacity"": 80000,
  ""tribunes"": [
    {
      ""code"": ""N"",
      ""name"": ""North Tribune"",
      ""description"": ""Main tribune with multiple tiers"",
      ""rings"": [
        {
          ""number"": 1,
          ""name"": ""Lower Ring"",
          ""priceMultiplier"": 1.2,
          ""sectors"": [
            {
              ""code"": ""NA"",
              ""name"": ""North Lower A"",
              ""type"": ""standard"",
              ""rows"": 30,
              ""seatsPerRow"": 25,
              ""priceCategory"": ""A""
            },
            {
              ""code"": ""NB"",
              ""name"": ""North Lower B"",
              ""type"": ""standard"",
              ""rows"": 30,
              ""seatsPerRow"": 25,
              ""priceCategory"": ""A""
            }
          ]
        },
        {
          ""number"": 2,
          ""name"": ""VIP Ring"",
          ""priceMultiplier"": 3.0,
          ""sectors"": [
            {
              ""code"": ""NV1"",
              ""name"": ""VIP Box 1"",
              ""type"": ""vip"",
              ""rows"": 3,
              ""seatsPerRow"": 10,
              ""priceCategory"": ""VIP""
            },
            {
              ""code"": ""NV2"",
              ""name"": ""VIP Box 2"",
              ""type"": ""vip"",
              ""rows"": 3,
              ""seatsPerRow"": 10,
              ""priceCategory"": ""VIP""
            }
          ]
        },
        {
          ""number"": 3,
          ""name"": ""Upper Ring"",
          ""sectors"": [
            {
              ""code"": ""NU"",
              ""name"": ""North Upper"",
              ""type"": ""standard"",
              ""rows"": 40,
              ""seatsPerRow"": 30,
              ""priceCategory"": ""B""
            }
          ]
        }
      ]
    }
  ]
}",
        _ => ""
    };

    // DTO class for validation
    public class StadiumStructureDto
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int? Capacity { get; set; }
        public List<TribuneDto> Tribunes { get; set; } = new();
    }

    public class TribuneDto
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public List<RingDto> Rings { get; set; } = new();
    }

    public class RingDto
    {
        public int Number { get; set; }
        public string Name { get; set; } = "";
        public double? PriceMultiplier { get; set; }
        public List<SectorDto> Sectors { get; set; } = new();
    }

    public class SectorDto
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Type { get; set; }
        public int Rows { get; set; }
        public int SeatsPerRow { get; set; }
        public int StartRow { get; set; } = 1;
        public int StartSeat { get; set; } = 1;
        public string? PriceCategory { get; set; }
    }
}