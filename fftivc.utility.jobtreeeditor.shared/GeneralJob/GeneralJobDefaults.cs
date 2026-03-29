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
        RightNeighbor = GeneralJobKey.Chemist,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.Knight,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Archer,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.Archer,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Chemist,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.None,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Dragoon,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.None,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultChemist = new()
    {
        Key = 1,
        DisplayName = "Chemist",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [],
        RightNeighbor = GeneralJobKey.Squire,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.BlackMage,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.WhiteMage,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.WhiteMage,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Squire,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.None,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Bard,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.None,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultKnight = new() 
    {
        Key = 2, 
        DisplayName = "Knight", 
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.Squire, 2, LevelRequirementPosition.Unknown5)],
        RightNeighbor = GeneralJobKey.WhiteMage,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.None,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Monk,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.None,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Archer,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Squire,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Squire,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.Chemist,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultArcher = new()
    {
        Key = 3,
        DisplayName = "Archer",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.Squire, 2, LevelRequirementPosition.Unknown6)],
        RightNeighbor = GeneralJobKey.Knight,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.None,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Thief,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.None,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.BlackMage,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.None,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Squire,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.Squire,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultMonk = new()
    {
        Key = 4,
        DisplayName = "Monk",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.Knight, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = GeneralJobKey.Mystic,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.Orator,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Geomancer,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.Ninja,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.TimeMage,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Archer,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Knight,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.WhiteMage,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultWhiteMage = new()
    {
        Key = 5,
        DisplayName = "White Mage",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.Chemist, 2, LevelRequirementPosition.Unknown6)],
        RightNeighbor = GeneralJobKey.BlackMage,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.Arithmetician,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Mystic,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.Monk,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Knight,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Squire,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Chemist,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.Chemist,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultBlackMage = new()
    {
        Key = 6,
        DisplayName = "Black Mage",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.Chemist, 2, LevelRequirementPosition.Unknown5)],
        RightNeighbor = GeneralJobKey.Archer,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.None,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.TimeMage,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.Arithmetician,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.WhiteMage,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Chemist,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Chemist,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.None,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultTimeMage = new()
    {
        Key = 7,
        DisplayName = "Time Mage",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.BlackMage, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = GeneralJobKey.Monk,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.None,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Summoner,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.Bard,
        DownLeftFallback = GeneralJobKey.Orator,
        LeftNeighbor = GeneralJobKey.Arithmetician,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.WhiteMage,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.BlackMage,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.None,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultSummoner = new()
    {
        Key = 8,
        DisplayName = "Summoner",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.TimeMage, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = GeneralJobKey.Thief,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.None,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Mime,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.Bard,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Bard,
        LeftFallback = GeneralJobKey.Orator,
        UpLeftNeighbor = GeneralJobKey.Arithmetician,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.TimeMage,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.None,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultThief = new()
    {
        Key = 9,
        DisplayName = "Thief",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.Archer, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = GeneralJobKey.Ninja,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.Dragoon,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Dragoon,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.None,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Summoner,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.None,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Archer,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.Knight,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultOrator = new()
    {
        Key = 10,
        DisplayName = "Orator",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.Mystic, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = GeneralJobKey.Bard,
        RightFallback = GeneralJobKey.Summoner,
        DownRightNeighbor = GeneralJobKey.None,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Mime,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.Mime,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Geomancer,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Monk,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Mystic,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.Arithmetician,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultMystic = new()
    {
        Key = 11,
        DisplayName = "Mystic",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.WhiteMage, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = GeneralJobKey.Arithmetician,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.Bard,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Orator,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.None,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Monk,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Knight,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.WhiteMage,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.BlackMage,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultGeomancer = new()
    {
        Key = 12,
        DisplayName = "Geomancer",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.Monk, 4, LevelRequirementPosition.Bottom)],
        RightNeighbor = GeneralJobKey.Orator,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.Mime,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Dancer,
        DownFallback = GeneralJobKey.Samurai,
        DownLeftNeighbor = GeneralJobKey.Dragoon,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Ninja,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Archer,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Monk,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.Mystic,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultDragoon = new()
    {
        Key = 13,
        DisplayName = "Dragoon",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(GeneralJobKey.Thief, 4, LevelRequirementPosition.Bottom)],
        RightNeighbor = GeneralJobKey.Samurai,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.None,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Squire,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.None,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Mime,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Thief,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Dancer,
        UpFallback = GeneralJobKey.Ninja,
        UpRightNeighbor = GeneralJobKey.Geomancer,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultSamurai = new()
    {
        Key = 14,
        DisplayName = "Samurai",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(GeneralJobKey.Knight, 4, LevelRequirementPosition.Right),
            new (GeneralJobKey.Monk, 5, LevelRequirementPosition.Right),
            new (GeneralJobKey.Dragoon, 2, LevelRequirementPosition.Right)
             ],
        RightNeighbor = GeneralJobKey.Mime,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.None,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Knight,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.None,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Dragoon,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Ninja,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Dancer,
        UpFallback = GeneralJobKey.Geomancer,
        UpRightNeighbor = GeneralJobKey.Orator,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultNinja = new()
    {
        Key = 15,
        DisplayName = "Ninja",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(GeneralJobKey.Archer, 4, LevelRequirementPosition.Right),
            new(GeneralJobKey.Thief, 5, LevelRequirementPosition.Right),
            new(GeneralJobKey.Geomancer, 2, LevelRequirementPosition.Left)
            ],
        RightNeighbor = GeneralJobKey.Geomancer,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.Dancer,
        DownRightFallback = GeneralJobKey.Samurai,
        DownNeighbor = GeneralJobKey.Dragoon,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.None,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Thief,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Archer,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Squire,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.Monk,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultArithmetician = new()
    {
        Key = 16,
        DisplayName = "Arithmetician",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(GeneralJobKey.WhiteMage, 5, LevelRequirementPosition.Right),
            new(GeneralJobKey.BlackMage, 5, LevelRequirementPosition.Left),
            new(GeneralJobKey.TimeMage, 4, LevelRequirementPosition.Left),
            new(GeneralJobKey.Mystic, 4, LevelRequirementPosition.Right)
             ],
        RightNeighbor = GeneralJobKey.TimeMage,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.Summoner,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Bard,
        DownFallback = GeneralJobKey.Chemist,
        DownLeftNeighbor = GeneralJobKey.Orator,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Mystic,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.WhiteMage,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Chemist,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.BlackMage,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultBard = new()
    {
        Key = 17,
        DisplayName = "Bard",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(GeneralJobKey.Summoner, 5, LevelRequirementPosition.Left),
            new(GeneralJobKey.Orator, 5, LevelRequirementPosition.Right)
             ],
        RightNeighbor = GeneralJobKey.Summoner,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.None,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Chemist,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.Mime,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Orator,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Mystic,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Arithmetician,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.TimeMage,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultDancer = new()
    {
        Key = 18,
        DisplayName = "Dancer",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(GeneralJobKey.Geomancer, 5, LevelRequirementPosition.Bottom),
            new(GeneralJobKey.Dragoon, 5, LevelRequirementPosition.Top)
            ],
        RightNeighbor = GeneralJobKey.Geomancer,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.Mime,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Dragoon,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.Dragoon,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Dragoon,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Ninja,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Geomancer,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.Orator,
        UpRightFallback = GeneralJobKey.None,
    };

    private static readonly GeneralJobRecord DefaultMime = new()
    {
        Key = 19,
        DisplayName = "Mime",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(GeneralJobKey.Squire, 8, LevelRequirementPosition.Right),
            new(GeneralJobKey.Chemist, 8, LevelRequirementPosition.Left),
            new(GeneralJobKey.Summoner, 5, LevelRequirementPosition.Bottom),
            new(GeneralJobKey.Orator, 5, LevelRequirementPosition.Bottom),
            new(GeneralJobKey.Geomancer, 5, LevelRequirementPosition.Right),
            new(GeneralJobKey.Dragoon, 5, LevelRequirementPosition.Bottom)
            ],
        RightNeighbor = GeneralJobKey.Dragoon,
        RightFallback = GeneralJobKey.None,
        DownRightNeighbor = GeneralJobKey.None,
        DownRightFallback = GeneralJobKey.None,
        DownNeighbor = GeneralJobKey.Squire,
        DownFallback = GeneralJobKey.None,
        DownLeftNeighbor = GeneralJobKey.None,
        DownLeftFallback = GeneralJobKey.None,
        LeftNeighbor = GeneralJobKey.Samurai,
        LeftFallback = GeneralJobKey.None,
        UpLeftNeighbor = GeneralJobKey.Geomancer,
        UpLeftFallback = GeneralJobKey.None,
        UpNeighbor = GeneralJobKey.Chemist,
        UpFallback = GeneralJobKey.None,
        UpRightNeighbor = GeneralJobKey.Orator,
        UpRightFallback = GeneralJobKey.None,
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
