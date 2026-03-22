namespace fftivc.utility.jobtreeeditor.shared.Enums;

/// <summary>
/// Controls where the level requirement label is displayed relative to the job sprite.
/// Values 5 and 6 behave inconsistently across different sprites.
/// </summary>
public enum LevelRequirementPosition
{
    Left = 1,
    Right = 2,
    Bottom = 3,
    Top = 4,
    Unknown5 = 5,
    Unknown6 = 6,
}
