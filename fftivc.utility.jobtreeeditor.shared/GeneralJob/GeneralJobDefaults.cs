using fftivc.utility.jobtreeeditor.shared.Enums;

namespace fftivc.utility.jobtreeeditor.shared.GeneralJob;

/// <summary>
/// Default GeneralJob data
/// </summary>
public class GeneralJobDefaults
{
    private static readonly int[] DefaultExp = { 0, 100, 200, 400, 700, 1100, 1600, 2200, 3000 };

    /// <summary>
    /// All 20 job records with their default values.
    /// </summary>
    public static List<GeneralJobRecord> CreateDefaults()
    {
        return
        [
            DefaultSquire.Clone(),
            DefaultChemist.Clone(),
            DefaultKnight.Clone(),
            DefaultArcher.Clone(),
            DefaultMonk.Clone(),
            DefaultWhiteMage.Clone() ,
            DefaultBlackMage.Clone(),
            DefaultTimeMage.Clone(),
            DefaultSummoner.Clone(),
            DefaultThief.Clone(),
            DefaultOrator.Clone(),
            DefaultMystic.Clone(),
            DefaultGeomancer.Clone(),
            DefaultDragoon.Clone(),
            DefaultSamurai.Clone(),
            DefaultNinja.Clone(),
            DefaultArithmetician.Clone(),
            DefaultBard.Clone(),
            DefaultDancer.Clone(),
            DefaultMime.Clone(),
        ];
    }

    private static readonly GeneralJobRecord DefaultSquire = new()
    {

        Key = 0,
        DisplayName = "Squire",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [],
        RightNeighbor = Job.Chemist,
        RightFallback = Job.None,
        DownRightNeighbor = Job.Knight,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Archer,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.Archer,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Chemist,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.None,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Dragoon,
        UpFallback = Job.None,
        UpRightNeighbor = Job.None,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultChemist = new()
    {
        Key = 1,
        DisplayName = "Chemist",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [],
        RightNeighbor = Job.Squire,
        RightFallback = Job.None,
        DownRightNeighbor = Job.BlackMage,
        DownRightFallback = Job.None,
        DownNeighbor = Job.WhiteMage,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.WhiteMage,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Squire,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.None,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Bard,
        UpFallback = Job.None,
        UpRightNeighbor = Job.None,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultKnight = new() 
    {
        Key = 2, 
        DisplayName = "Knight", 
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Squire, 2, LevelRequirementPosition.Unknown5)],
        RightNeighbor = Job.WhiteMage,
        RightFallback = Job.None,
        DownRightNeighbor = Job.None,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Monk,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.None,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Archer,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Squire,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Squire,
        UpFallback = Job.None,
        UpRightNeighbor = Job.Chemist,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultArcher = new()
    {
        Key = 3,
        DisplayName = "Archer",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Squire, 2, LevelRequirementPosition.Unknown6)],
        RightNeighbor = Job.Knight,
        RightFallback = Job.None,
        DownRightNeighbor = Job.None,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Thief,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.None,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.BlackMage,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.None,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Squire,
        UpFallback = Job.None,
        UpRightNeighbor = Job.Squire,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultMonk = new()
    {
        Key = 4,
        DisplayName = "Monk",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Knight, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Mystic,
        RightFallback = Job.None,
        DownRightNeighbor = Job.Orator,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Geomancer,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.Ninja,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.TimeMage,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Archer,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Knight,
        UpFallback = Job.None,
        UpRightNeighbor = Job.WhiteMage,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultWhiteMage = new()
    {
        Key = 5,
        DisplayName = "White Mage",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Chemist, 2, LevelRequirementPosition.Unknown6)],
        RightNeighbor = Job.BlackMage,
        RightFallback = Job.None,
        DownRightNeighbor = Job.Arithmetician,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Mystic,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.Monk,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Knight,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Squire,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Chemist,
        UpFallback = Job.None,
        UpRightNeighbor = Job.Chemist,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultBlackMage = new()
    {
        Key = 6,
        DisplayName = "Black Mage",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Chemist, 2, LevelRequirementPosition.Unknown5)],
        RightNeighbor = Job.Archer,
        RightFallback = Job.None,
        DownRightNeighbor = Job.None,
        DownRightFallback = Job.None,
        DownNeighbor = Job.TimeMage,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.Arithmetician,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.WhiteMage,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Chemist,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Chemist,
        UpFallback = Job.None,
        UpRightNeighbor = Job.None,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultTimeMage = new()
    {
        Key = 7,
        DisplayName = "Time Mage",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.BlackMage, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Monk,
        RightFallback = Job.None,
        DownRightNeighbor = Job.None,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Summoner,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.Bard,
        DownLeftFallback = Job.Orator,
        LeftNeighbor = Job.Arithmetician,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.WhiteMage,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.BlackMage,
        UpFallback = Job.None,
        UpRightNeighbor = Job.None,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultSummoner = new()
    {
        Key = 8,
        DisplayName = "Summoner",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.TimeMage, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Thief,
        RightFallback = Job.None,
        DownRightNeighbor = Job.None,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Mime,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.Bard,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Bard,
        LeftFallback = Job.Orator,
        UpLeftNeighbor = Job.Arithmetician,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.TimeMage,
        UpFallback = Job.None,
        UpRightNeighbor = Job.None,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultThief = new()
    {
        Key = 9,
        DisplayName = "Thief",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Archer, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Ninja,
        RightFallback = Job.None,
        DownRightNeighbor = Job.Dragoon,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Dragoon,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.None,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Summoner,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.None,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Archer,
        UpFallback = Job.None,
        UpRightNeighbor = Job.Knight,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultOrator = new()
    {
        Key = 10,
        DisplayName = "Orator",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Mystic, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Bard,
        RightFallback = Job.Summoner,
        DownRightNeighbor = Job.None,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Mime,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.Mime,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Geomancer,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Monk,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Mystic,
        UpFallback = Job.None,
        UpRightNeighbor = Job.Arithmetician,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultMystic = new()
    {
        Key = 11,
        DisplayName = "Mystic",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.WhiteMage, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Arithmetician,
        RightFallback = Job.None,
        DownRightNeighbor = Job.Bard,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Orator,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.None,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Monk,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Knight,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.WhiteMage,
        UpFallback = Job.None,
        UpRightNeighbor = Job.BlackMage,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultGeomancer = new()
    {
        Key = 12,
        DisplayName = "Geomancer",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Monk, 4, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Orator,
        RightFallback = Job.None,
        DownRightNeighbor = Job.Mime,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Dancer,
        DownFallback = Job.Samurai,
        DownLeftNeighbor = Job.Dragoon,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Ninja,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Archer,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Monk,
        UpFallback = Job.None,
        UpRightNeighbor = Job.Mystic,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultDragoon = new()
    {
        Key = 13,
        DisplayName = "Dragoon",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Thief, 4, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Samurai,
        RightFallback = Job.None,
        DownRightNeighbor = Job.None,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Squire,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.None,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Mime,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Thief,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Dancer,
        UpFallback = Job.Ninja,
        UpRightNeighbor = Job.Geomancer,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultSamurai = new()
    {
        Key = 14,
        DisplayName = "Samurai",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(Job.Knight, 4, LevelRequirementPosition.Right),
            new (Job.Monk, 5, LevelRequirementPosition.Right),
            new (Job.Dragoon, 2, LevelRequirementPosition.Right)
             ],
        RightNeighbor = Job.Mime,
        RightFallback = Job.None,
        DownRightNeighbor = Job.None,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Knight,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.None,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Dragoon,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Ninja,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Dancer,
        UpFallback = Job.Geomancer,
        UpRightNeighbor = Job.Orator,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultNinja = new()
    {
        Key = 15,
        DisplayName = "Ninja",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(Job.Archer, 4, LevelRequirementPosition.Right),
            new(Job.Thief, 5, LevelRequirementPosition.Right),
            new(Job.Geomancer, 2, LevelRequirementPosition.Left)
            ],
        RightNeighbor = Job.Geomancer,
        RightFallback = Job.None,
        DownRightNeighbor = Job.Dancer,
        DownRightFallback = Job.Samurai,
        DownNeighbor = Job.Dragoon,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.None,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Thief,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Archer,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Squire,
        UpFallback = Job.None,
        UpRightNeighbor = Job.Monk,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultArithmetician = new()
    {
        Key = 16,
        DisplayName = "Arithmetician",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(Job.WhiteMage, 5, LevelRequirementPosition.Right),
            new(Job.BlackMage, 5, LevelRequirementPosition.Left),
            new(Job.TimeMage, 4, LevelRequirementPosition.Left),
            new(Job.Mystic, 4, LevelRequirementPosition.Right)
             ],
        RightNeighbor = Job.TimeMage,
        RightFallback = Job.None,
        DownRightNeighbor = Job.Summoner,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Bard,
        DownFallback = Job.Chemist,
        DownLeftNeighbor = Job.Orator,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Mystic,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.WhiteMage,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Chemist,
        UpFallback = Job.None,
        UpRightNeighbor = Job.BlackMage,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultBard = new()
    {
        Key = 17,
        DisplayName = "Bard",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(Job.Summoner, 5, LevelRequirementPosition.Left),
            new(Job.Orator, 5, LevelRequirementPosition.Right)
             ],
        RightNeighbor = Job.Summoner,
        RightFallback = Job.None,
        DownRightNeighbor = Job.None,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Chemist,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.Mime,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Orator,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Mystic,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Arithmetician,
        UpFallback = Job.None,
        UpRightNeighbor = Job.TimeMage,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultDancer = new()
    {
        Key = 18,
        DisplayName = "Dancer",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(Job.Geomancer, 5, LevelRequirementPosition.Bottom),
            new(Job.Dragoon, 5, LevelRequirementPosition.Top)
            ],
        RightNeighbor = Job.Geomancer,
        RightFallback = Job.None,
        DownRightNeighbor = Job.Mime,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Dragoon,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.Dragoon,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Dragoon,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Ninja,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Geomancer,
        UpFallback = Job.None,
        UpRightNeighbor = Job.Orator,
        UpRightFallback = Job.None,
    };

    private static readonly GeneralJobRecord DefaultMime = new()
    {
        Key = 19,
        DisplayName = "Mime",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(Job.Squire, 8, LevelRequirementPosition.Right),
            new(Job.Chemist, 8, LevelRequirementPosition.Left),
            new(Job.Summoner, 5, LevelRequirementPosition.Bottom),
            new(Job.Orator, 5, LevelRequirementPosition.Bottom),
            new(Job.Geomancer, 5, LevelRequirementPosition.Right),
            new(Job.Dragoon, 5, LevelRequirementPosition.Bottom)
            ],
        RightNeighbor = Job.Dragoon,
        RightFallback = Job.None,
        DownRightNeighbor = Job.None,
        DownRightFallback = Job.None,
        DownNeighbor = Job.Squire,
        DownFallback = Job.None,
        DownLeftNeighbor = Job.None,
        DownLeftFallback = Job.None,
        LeftNeighbor = Job.Samurai,
        LeftFallback = Job.None,
        UpLeftNeighbor = Job.Geomancer,
        UpLeftFallback = Job.None,
        UpNeighbor = Job.Chemist,
        UpFallback = Job.None,
        UpRightNeighbor = Job.Orator,
        UpRightFallback = Job.None,
    };


    /// <summary>
    /// Lookup table: GeneralJob key → English name.
    /// </summary>
    public static readonly Dictionary<int, string> JobNames = new()
    {
        { 0, "Squire" }, 
        { 1, "Chemist" }, 
        { 2, "Knight" }, 
        { 3, "Archer" },
        { 4, "Monk" }, 
        { 5, "White Mage" }, 
        { 6, "Black Mage" }, 
        { 7, "Time Mage" },
        { 8, "Summoner" }, 
        { 9, "Thief" }, 
        { 10, "Orator" }, 
        { 11, "Mystic" },
        { 12, "Geomancer" }, 
        { 13, "Dragoon" }, 
        { 14, "Samurai" }, 
        { 15, "Ninja" },
        { 16, "Arithmetician" }, 
        { 17, "Bard" }, 
        { 18, "Dancer" }, 
        { 19, "Mime" },
        { 20, "(None)" },
    };

    /// <summary>
    /// Format a job key as "Key - Name" for display in dropdowns.
    /// </summary>
    public static string FormatJobRef(int key)
    {
        return JobNames.TryGetValue(key, out var name)
            ? $"{key} - {name}"
            : $"{key} - ???";
    }
}
