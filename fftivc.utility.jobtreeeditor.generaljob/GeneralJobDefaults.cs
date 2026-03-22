using fftivc.utility.jobtreeeditor.generaljob.Enums;

namespace fftivc.utility.jobtreeeditor.generaljob;

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
            DefaultSquire,
            DefaultChemist,
            DefaultKnight,
            DefaultArcher,
            DefaultMonk,
            DefaultWhiteMage ,
            DefaultBlackMage,
            DefaultTimeMage,
            DefaultSummoner,
            DefaultThief,
            DefaultOrator,
            DefaultMystic,
            DefaultGeomancer,
            DefaultDragoon,
            DefaultSamurai,
            DefaultNinja,
            DefaultArithmetician,
            DefaultBard,
            DefaultDancer,
            DefaultMime,
        ];

    }

    private static readonly GeneralJobRecord DefaultSquire = new()
    {

        Key = 0,
        Name = "Squire",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [],
        RightNeighbor = Job.Chemist,
        DownNeighbor = Job.Archer,
        LeftNeighbor = Job.Chemist,
        UpNeighbor = Job.Dragoon,
    };

    private static readonly GeneralJobRecord DefaultChemist = new()
    {
        Key = 1,
        Name = "Chemist",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [],
        RightNeighbor = Job.Squire,
        DownNeighbor = Job.WhiteMage,
        LeftNeighbor = Job.Squire,
        UpNeighbor = Job.Bard,
    };

    private static readonly GeneralJobRecord DefaultKnight = new() 
    {
        Key = 2, 
        Name = "Knight", 
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Squire, 2, LevelRequirementPosition.Unknown5)],
        RightNeighbor = Job.WhiteMage, 
        DownNeighbor = Job.Monk, 
        LeftNeighbor = Job.Archer, 
        UpNeighbor = Job.Squire,
    };

    private static readonly GeneralJobRecord DefaultArcher = new()
    {
        Key = 3,
        Name = "Archer",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Squire, 2, LevelRequirementPosition.Unknown6)],
        RightNeighbor = Job.Knight,
        DownNeighbor = Job.Thief,
        LeftNeighbor = Job.BlackMage,
        UpNeighbor = Job.Squire,
    };

    private static readonly GeneralJobRecord DefaultMonk = new()
    {
        Key = 4,
        Name = "Monk",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Knight, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Mystic,
        DownNeighbor = Job.Geomancer,
        LeftNeighbor = Job.TimeMage,
        UpNeighbor = Job.Knight,
    };

    private static readonly GeneralJobRecord DefaultWhiteMage = new()
    {
        Key = 5,
        Name = "White Mage",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Chemist, 2, LevelRequirementPosition.Unknown6)],
        RightNeighbor = Job.BlackMage,
        DownNeighbor = Job.Mystic,
        LeftNeighbor = Job.Knight,
        UpNeighbor = Job.Chemist,
    };

    private static readonly GeneralJobRecord DefaultBlackMage = new()
    {
        Key = 6,
        Name = "Black Mage",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Chemist, 2, LevelRequirementPosition.Unknown5)],
        RightNeighbor = Job.Archer,
        DownNeighbor = Job.TimeMage,
        LeftNeighbor = Job.WhiteMage,
        UpNeighbor = Job.Chemist,
    };

    private static readonly GeneralJobRecord DefaultTimeMage = new()
    {
        Key = 7,
        Name = "Time Mage",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.BlackMage, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Monk,
        DownNeighbor = Job.Summoner,
        LeftNeighbor = Job.Arithmetician,
        UpNeighbor = Job.BlackMage,
    };

    private static readonly GeneralJobRecord DefaultSummoner = new()
    {
        Key = 8,
        Name = "Summoner",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.TimeMage, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Thief,
        DownNeighbor = Job.Mime,
        LeftNeighbor = Job.Bard,
        UpNeighbor = Job.TimeMage,
    };

    private static readonly GeneralJobRecord DefaultThief = new()
    {
        Key = 9,
        Name = "Thief",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Archer, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Ninja,
        DownNeighbor = Job.Dragoon,
        LeftNeighbor = Job.Summoner,
        UpNeighbor = Job.Archer,
    };

    private static readonly GeneralJobRecord DefaultOrator = new()
    {
        Key = 10,
        Name = "Orator",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Mystic, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Bard,
        DownNeighbor = Job.Mime,
        LeftNeighbor = Job.Geomancer,
        UpNeighbor = Job.Mystic,
    };

    private static readonly GeneralJobRecord DefaultMystic = new()
    {
        Key = 11,
        Name = "Mystic",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.WhiteMage, 3, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Arithmetician,
        DownNeighbor = Job.Orator,
        LeftNeighbor = Job.Monk,
        UpNeighbor = Job.WhiteMage,
    };

    private static readonly GeneralJobRecord DefaultGeomancer = new()
    {
        Key = 12,
        Name = "Geomancer",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Monk, 4, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Orator,
        DownNeighbor = Job.Dancer,
        LeftNeighbor = Job.Ninja,
        UpNeighbor = Job.Monk,
    };

    private static readonly GeneralJobRecord DefaultDragoon = new()
    {
        Key = 13,
        Name = "Dragoon",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [new(Job.Thief, 4, LevelRequirementPosition.Bottom)],
        RightNeighbor = Job.Samurai,
        DownNeighbor = Job.Squire,
        LeftNeighbor = Job.Mime,
        UpNeighbor = Job.Dancer,
    };

    private static readonly GeneralJobRecord DefaultSamurai = new()
    {
        Key = 14,
        Name = "Samurai",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
             new(Job.Knight, 4, LevelRequirementPosition.Right),
                    new (Job.Monk, 5, LevelRequirementPosition.Right),
                    new (Job.Dragoon, 2, LevelRequirementPosition.Right)
             ],
        RightNeighbor = Job.Mime,
        DownNeighbor = Job.Knight,
        LeftNeighbor = Job.Dragoon,
        UpNeighbor = Job.Dancer,
    };

    private static readonly GeneralJobRecord DefaultNinja = new()
    {
        Key = 15,
        Name = "Ninja",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(Job.Archer, 4, LevelRequirementPosition.Right),
            new(Job.Thief, 5, LevelRequirementPosition.Right),
            new(Job.Geomancer, 2, LevelRequirementPosition.Left)
            ],
        RightNeighbor = Job.Geomancer,
        DownNeighbor = Job.Dragoon,
        LeftNeighbor = Job.Thief,
        UpNeighbor = Job.Squire,
    };

    private static readonly GeneralJobRecord DefaultArithmetician = new()
    {
        Key = 16,
        Name = "Arithmetician",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(Job.WhiteMage, 5, LevelRequirementPosition.Right),
            new(Job.BlackMage, 5, LevelRequirementPosition.Left),
            new(Job.TimeMage, 4, LevelRequirementPosition.Left),
            new(Job.Mystic, 4, LevelRequirementPosition.Right)
             ],
        RightNeighbor = Job.TimeMage,
        DownNeighbor = Job.Bard,
        LeftNeighbor = Job.Mystic,
        UpNeighbor = Job.Chemist,
    };

    private static readonly GeneralJobRecord DefaultBard = new()
    {
        Key = 17,
        Name = "Bard",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(Job.Summoner, 5, LevelRequirementPosition.Left),
            new(Job.Orator, 5, LevelRequirementPosition.Right)
             ],
        RightNeighbor = Job.Summoner,
        DownNeighbor = Job.Chemist,
        LeftNeighbor = Job.Orator,
        UpNeighbor = Job.Arithmetician,
    };

    private static readonly GeneralJobRecord DefaultDancer = new()
    {
        Key = 18,
        Name = "Dancer",
        Comment = "",
        RequiredJobExp = [.. DefaultExp],
        Prerequisites = [
            new(Job.Geomancer, 5, LevelRequirementPosition.Bottom),
            new(Job.Dragoon, 5, LevelRequirementPosition.Top)
            ],
        RightNeighbor = Job.Geomancer,
        DownNeighbor = Job.Dragoon,
        LeftNeighbor = Job.Dragoon,
        UpNeighbor = Job.Geomancer,
    };

    private static readonly GeneralJobRecord DefaultMime = new()
    {
        Key = 19,
        Name = "Mime",
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
        DownNeighbor = Job.Squire,
        LeftNeighbor = Job.Samurai,
        UpNeighbor = Job.Chemist
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
    /// Format a job key as "Key — Name" for display in dropdowns.
    /// </summary>
    public static string FormatJobRef(int key)
    {
        return JobNames.TryGetValue(key, out var name)
            ? $"{key} — {name}"
            : $"{key} — ???";
    }
}
