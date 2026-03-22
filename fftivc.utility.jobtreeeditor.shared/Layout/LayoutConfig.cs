using fftivc.utility.jobtreeeditor.shared.Enums;
using fftivc.utility.jobtreeeditor.shared.GeneralJob;
using fftivc.utility.jobtreeeditor.uib;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace fftivc.utility.jobtreeeditor.shared.Layout;

/// <summary>
/// A complete layout configuration that can be saved/loaded as JSON.
/// </summary>
public class LayoutConfig
{
    [JsonPropertyName("version")]
    public int Version { get; set; } = 1;

    [JsonPropertyName("name")]
    public string Name { get; set; } = "Custom Layout";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("jobs")]
    public List<LayoutJob> Jobs { get; set; } = new();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
    };

    /// <summary>
    /// Build a layout from the current UIB positions and GeneralJob records.
    /// </summary>
    public static LayoutConfig Export(JobTreeUib uib, List<GeneralJobRecord> records, string name = "Exported Layout")
    {
        var config = new LayoutConfig { Name = name };

        foreach (var rec in records)
        {
            // Find the matching UIB slot by GeneralJobKey
            var slot = UibConstants.Jobs.FirstOrDefault(s => s.GeneralJobKey == rec.Key);
            var pos = slot != null ? uib.ReadPosition(slot) : new JobPosition(0, 0);

            config.Jobs.Add(new LayoutJob
            {
                Key = rec.Key,
                Name = rec.Name,
                Position = new LayoutPosition { X = pos.X, Y = pos.Y },
                Comment = rec.Comment,
                RequiredJobExp = [.. rec.RequiredJobExp],
                Prerequisites = [.. rec.Prerequisites.Select(p => new LayoutPrerequisite
                {
                    JobKey = (int)p.RequiredJobId,
                    Level = p.RequiredLevel,
                    RequirementPosition = p.LevelRequirementPosition,
                })],
                RightNeighbor = (int)rec.RightNeighbor,
                DownNeighbor = (int)rec.DownNeighbor,
                LeftNeighbor = (int)rec.LeftNeighbor,
                UpNeighbor = (int)rec.UpNeighbor,
            });
        }

        return config;
    }

    /// <summary>
    /// Build a layout from the hardcoded defaults (no UIB needed).
    /// </summary>
    public static LayoutConfig ExportDefaults()
    {
        var config = new LayoutConfig
        {
            Name = "Default FFT Job Tree",
            Description = "Original unmodified job positions and GeneralJob data",
        };

        var defaults = GeneralJobDefaults.CreateDefaults();
        foreach (var rec in defaults)
        {
            var slot = UibConstants.Jobs.FirstOrDefault(s => s.GeneralJobKey == rec.Key);

            config.Jobs.Add(new LayoutJob
            {
                Key = rec.Key,
                Name = rec.Name,
                Position = new LayoutPosition
                {
                    X = slot?.DefaultX ?? 0,
                    Y = slot?.DefaultY ?? 0,
                },
                Comment = rec.Comment,
                RequiredJobExp = new List<int>(rec.RequiredJobExp),
                Prerequisites = rec.Prerequisites.Select(p => new LayoutPrerequisite
                {
                    JobKey = (int)p.RequiredJobId,
                    Level = p.RequiredLevel,
                    RequirementPosition = p.LevelRequirementPosition,
                }).ToList(),
                RightNeighbor = (int)rec.RightNeighbor,
                DownNeighbor = (int)rec.DownNeighbor,
                LeftNeighbor = (int)rec.LeftNeighbor,
                UpNeighbor = (int)rec.UpNeighbor,
            });
        }

        return config;
    }

    /// <summary>
    /// Apply this layout to a UIB file and GeneralJob records.
    /// Returns lists of what was applied and what was skipped.
    /// </summary>
    public (List<string> applied, List<string> skipped) Apply(JobTreeUib uib, List<GeneralJobRecord> records)
    {
        var applied = new List<string>();
        var skipped = new List<string>();

        foreach (var entry in Jobs)
        {
            // Apply position to UIB
            var slot = UibConstants.Jobs.FirstOrDefault(s => s.GeneralJobKey == entry.Key);
            if (slot == null)
            {
                skipped.Add($"Key {entry.Key} ({entry.Name}) - no UIB slot");
                continue;
            }

            uib.WritePosition(slot, new JobPosition(entry.Position.X, entry.Position.Y));

            // Apply GeneralJob data
            var rec = records.FirstOrDefault(r => r.Key == entry.Key);
            if (rec == null)
            {
                skipped.Add($"Key {entry.Key} ({entry.Name}) - no GeneralJob record");
                continue;
            }

            rec.Comment = entry.Comment;
            rec.RequiredJobExp = new List<int>(entry.RequiredJobExp);
            rec.Prerequisites = entry.Prerequisites.Select(p =>
                new JobPrerequisite(
                    (Job)p.JobKey,
                    p.Level,
                    p.RequirementPosition
                )).ToList();
            rec.RightNeighbor = (Job)entry.RightNeighbor;
            rec.DownNeighbor = (Job)entry.DownNeighbor;
            rec.LeftNeighbor = (Job)entry.LeftNeighbor;
            rec.UpNeighbor = (Job)entry.UpNeighbor;

            applied.Add($"{entry.Key} — {entry.Name}");
        }

        return (applied, skipped);
    }

    public void SaveToFile(string path)
    {
        string? dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        string json = JsonSerializer.Serialize(this, JsonOptions);
        File.WriteAllText(path, json);
    }

    public static LayoutConfig LoadFromFile(string path)
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<LayoutConfig>(json, JsonOptions)
            ?? throw new InvalidDataException("Failed to parse layout JSON.");
    }
}
