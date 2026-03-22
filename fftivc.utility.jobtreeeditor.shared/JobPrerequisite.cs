using fftivc.utility.jobtreeeditor.shared.Enums;

namespace fftivc.utility.jobtreeeditor.shared;

/// <summary>
/// A single prerequisite requirement: a job at a certain level, displayed in a certain position.
/// </summary>
public class JobPrerequisite(Job requiredJobId, int requiredLevel, LevelRequirementPosition levelRequirementPosition)
{
    /// <summary>GeneralJob key of the required job.</summary>
    public Job RequiredJobId { get; set; } = requiredJobId;

    /// <summary>Level required in that job.</summary>
    public int RequiredLevel { get; set; } = requiredLevel;

    /// <summary>Where the level requirement label appears relative to the sprite.</summary>
    public LevelRequirementPosition LevelRequirementPosition { get; set; } = levelRequirementPosition;

    public JobPrerequisite Clone() => new(RequiredJobId, RequiredLevel, LevelRequirementPosition);

    /// <summary>
    /// Lookup table: GeneralJob key → English name.
    /// </summary>
    public static readonly Dictionary<int, string> JobNames = new()
    {
        { 0, "Squire" }, { 1, "Chemist" }, { 2, "Knight" }, { 3, "Archer" },
        { 4, "Monk" }, { 5, "White Mage" }, { 6, "Black Mage" }, { 7, "Time Mage" },
        { 8, "Summoner" }, { 9, "Thief" }, { 10, "Orator" }, { 11, "Mystic" },
        { 12, "Geomancer" }, { 13, "Dragoon" }, { 14, "Samurai" }, { 15, "Ninja" },
        { 16, "Arithmetician" }, { 17, "Bard" }, { 18, "Dancer" }, { 19, "Mime" },
        { 20, "(None)" },
    };

    /// <summary>
    /// Format a job key as "Key — Name" for display in dropdowns.
    /// </summary>
    public static string FormatJobRef(int key)
    {
        return JobNames.TryGetValue(key, out var name)
            ? $"{key} — {name}"
            : $"{key} — ???";
    }
}
