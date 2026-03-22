using System.Text.Json;
using System.Text.Json.Serialization;

namespace fftivc.utility.jobtreeeditor.uib;

/// <summary>
/// A complete layout configuration that can be saved/loaded as JSON.
/// </summary>
public class LayoutConfig
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "Custom Layout";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("positions")]
    public List<LayoutEntry> Positions { get; set; } = [];

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Create a layout config from the current state of a UIB file.
    /// </summary>
    public static LayoutConfig FromUibFile(JobTreeUib uib, string name = "Exported Layout")
    {
        var config = new LayoutConfig { Name = name };
        foreach (var slot in UibConstants.Jobs)
        {
            var pos = uib.ReadPosition(slot);
            config.Positions.Add(new LayoutEntry
            {
                Job = slot.Name,
                X = pos.X,
                Y = pos.Y,
            });
        }
        return config;
    }

    /// <summary>
    /// Create a layout config from the default (unmodified) positions.
    /// </summary>
    public static LayoutConfig FromDefaults()
    {
        var config = new LayoutConfig
        {
            Name = "Default FFT Job Tree",
            Description = "Original unmodified job positions",
        };
        foreach (var slot in UibConstants.Jobs)
        {
            config.Positions.Add(new LayoutEntry
            {
                Job = slot.Name,
                X = slot.DefaultX,
                Y = slot.DefaultY,
            });
        }
        return config;
    }

    /// <summary>
    /// Apply this layout config to a UIB file.
    /// Only applies entries whose job name matches a known slot.
    /// Returns a list of jobs that were applied and any that were skipped.
    /// </summary>
    public (List<string> applied, List<string> skipped) ApplyTo(JobTreeUib uib)
    {
        var applied = new List<string>();
        var skipped = new List<string>();

        foreach (var entry in Positions)
        {
            var slot = UibConstants.Jobs.FirstOrDefault(
                s => s.Name.Equals(entry.Job, StringComparison.OrdinalIgnoreCase));

            if (slot == null)
            {
                skipped.Add(entry.Job);
                continue;
            }

            uib.WritePosition(slot, new JobPosition(entry.X, entry.Y));
            applied.Add(entry.Job);
        }

        return (applied, skipped);
    }

    /// <summary>
    /// Save to a JSON file.
    /// </summary>
    public void SaveToFile(string path)
    {
        string? dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        string json = JsonSerializer.Serialize(this, JsonOptions);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Load from a JSON file.
    /// </summary>
    public static LayoutConfig LoadFromFile(string path)
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<LayoutConfig>(json, JsonOptions)
            ?? throw new InvalidDataException("Failed to parse layout JSON.");
    }
}
