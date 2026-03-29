using fftivc.utility.jobtreeeditor.shared.Enums;
using fftivc.utility.jobtreeeditor.shared.GeneralJob;
using fftivc.utility.jobtreeeditor.uib;
using System.Text.Json;
using System.Text.Json.Serialization;
using static fftivc.utility.jobtreeeditor.uib.UibConstants;

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
    public List<LayoutJob> Jobs { get; set; } = [];

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

            string slotBinding = "Unknown";
            if (slot != null)
            {
                var nameRef = uib.ReadNameRef(slot);
                if (AddressToSlotName.TryGetValue(nameRef.Value, out var slotName))
                    slotBinding = slotName.ToString();
            }

            config.Jobs.Add(new LayoutJob
            {
                Key = rec.Key,
                DisplayName = rec.DisplayName,
                Position = new LayoutPosition { X = pos.X, Y = pos.Y },
                UibSlotBinding = slotBinding,
                Comment = rec.Comment,
                RequiredJobExp = [.. rec.RequiredJobExp],
                Prerequisites = [.. rec.Prerequisites.Select(p => new LayoutPrerequisite
                {
                    JobKey = (int)p.RequiredJobId,
                    Level = p.RequiredLevel,
                    RequirementPosition = p.LevelRequirementPosition,
                })],
                Neighbors = 
                {
                    RightNeighbor = (int)rec.RightNeighbor,
                    RightFallback = (int)rec.RightFallback,

                    DownRightNeighbor = (int)rec.DownRightNeighbor,
                    DownRightFallback = (int)rec.DownRightFallback,

                    DownNeighbor = (int)rec.DownNeighbor,
                    DownFallback = (int)rec.DownFallback,

                    DownLeftNeighbor = (int)rec.DownLeftNeighbor,
                    DownLeftFallback = (int)rec.DownLeftFallback,
                    
                    LeftNeighbor = (int)rec.LeftNeighbor,
                    LeftFallback = (int)rec.LeftFallback,

                    UpLeftNeighbor = (int)rec.UpLeftNeighbor,
                    UpLeftFallback = (int)rec.UpLeftFallback,
                    
                    UpNeighbor = (int)rec.UpNeighbor,
                    UpFallback = (int)rec.UpFallback,

                    UpRightNeighbor = (int)rec.UpRightNeighbor,
                    UpRightFallback = (int)rec.UpRightFallback,
                }
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
            string slotBinding = "Unknown";

            if (slot != null && AddressToSlotName
                .TryGetValue(slot.DefaultNameRef, out var slotName))
            {
                slotBinding = slotName.ToString();
            }

            config.Jobs.Add(new LayoutJob
            {
                Key = rec.Key,
                DisplayName = rec.DisplayName,
                UibSlotBinding = slotBinding,
                Position = new LayoutPosition
                {
                    X = slot?.DefaultX ?? 0,
                    Y = slot?.DefaultY ?? 0,
                },
                Comment = rec.Comment,
                RequiredJobExp = [.. rec.RequiredJobExp],
                Prerequisites = [.. rec.Prerequisites.Select(p => new LayoutPrerequisite
                {
                    JobKey = (int)p.RequiredJobId,
                    Level = p.RequiredLevel,
                    RequirementPosition = p.LevelRequirementPosition,
                })],
                Neighbors = new LayoutNeighbors 
                {
                    RightNeighbor = (int)rec.RightNeighbor,
                    RightFallback = (int)rec.RightFallback,

                    DownRightNeighbor = (int)rec.DownRightNeighbor,
                    DownRightFallback = (int)rec.DownRightFallback,

                    DownNeighbor = (int)rec.DownNeighbor,
                    DownFallback = (int)rec.DownFallback,

                    DownLeftNeighbor = (int)rec.DownLeftNeighbor,
                    DownLeftFallback = (int)rec.DownLeftFallback,

                    LeftNeighbor = (int)rec.LeftNeighbor,
                    LeftFallback = (int)rec.LeftFallback,

                    UpLeftNeighbor = (int)rec.UpLeftNeighbor,
                    UpLeftFallback = (int)rec.UpLeftFallback,

                    UpNeighbor = (int)rec.UpNeighbor,
                    UpFallback = (int)rec.UpFallback,

                    UpRightNeighbor = (int)rec.UpRightNeighbor,
                    UpRightFallback = (int)rec.UpRightFallback,
                }
            });
        }

        return config;
    }

    /// <summary>
    /// Apply this layout to a UIB file and GeneralJob records.
    /// Returns lists of what was applied and what was skipped.
    /// </summary>
    public (List<string> applied, List<string> skipped) Apply(JobTreeUib uib, List<GeneralJobRecord> generalJobRecords)
    {
        var applied = new List<string>();
        var skipped = new List<string>();

        foreach (var entry in Jobs)
        {
            // Apply position to UIB
            var slot = UibConstants.Jobs.FirstOrDefault(s => s.GeneralJobKey == entry.Key);
            if (slot == null)
            {
                skipped.Add($"Key {entry.Key} ({entry.DisplayName}) - no UIB slot");
                continue;
            }

            var slotName = Enum.Parse<UibSlotName>(entry.UibSlotBinding);
            var nameRef = new JobNameRef(SlotNameToAddress[slotName]);

            uib.WriteNameRef(slot, nameRef);
            uib.WritePosition(slot, new JobPosition(entry.Position.X, entry.Position.Y));

            // Apply GeneralJob data
            var generalJob = generalJobRecords.FirstOrDefault(r => r.Key == entry.Key);
            if (generalJob == null)
            {
                skipped.Add($"Key {entry.Key} ({entry.DisplayName}) - no GeneralJob record");
                continue;
            }

            generalJob.Comment = entry.Comment;
            generalJob.RequiredJobExp = [.. entry.RequiredJobExp];
            generalJob.Prerequisites = [.. entry.Prerequisites.Select(p =>
                new JobPrerequisite(
                    (GeneralJobKey)p.JobKey,
                    p.Level,
                    p.RequirementPosition
                ))];

            generalJob.RightNeighbor = (GeneralJobKey)entry.Neighbors.RightNeighbor;
            generalJob.RightFallback = (GeneralJobKey)entry.Neighbors.RightFallback;

            generalJob.DownRightNeighbor = (GeneralJobKey)entry.Neighbors.DownRightNeighbor;
            generalJob.DownRightFallback = (GeneralJobKey)entry.Neighbors.UpRightFallback;

            generalJob.DownNeighbor = (GeneralJobKey)entry.Neighbors.DownNeighbor;
            generalJob.DownFallback = (GeneralJobKey)entry.Neighbors.DownFallback;

            generalJob.DownLeftNeighbor = (GeneralJobKey)entry.Neighbors.DownLeftNeighbor;
            generalJob.DownLeftFallback = (GeneralJobKey)entry.Neighbors.DownLeftFallback;

            generalJob.LeftNeighbor = (GeneralJobKey)entry.Neighbors.LeftNeighbor;
            generalJob.LeftFallback = (GeneralJobKey)entry.Neighbors.LeftFallback;

            generalJob.UpLeftNeighbor = (GeneralJobKey)entry.Neighbors.UpLeftNeighbor;
            generalJob.UpLeftFallback = (GeneralJobKey)entry.Neighbors.UpLeftFallback;

            generalJob.UpNeighbor = (GeneralJobKey)entry.Neighbors.UpNeighbor;
            generalJob.UpFallback = (GeneralJobKey)entry.Neighbors.UpFallback;

            generalJob.UpRightNeighbor = (GeneralJobKey)entry.Neighbors.UpRightNeighbor;
            generalJob.UpRightFallback = (GeneralJobKey)entry.Neighbors.UpRightFallback;

            applied.Add($"{entry.Key} - {entry.DisplayName}");
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
