namespace fftivc.utility.jobtreeeditor.shared.JobNeedLevel;

public class JobNeedLevelDefaults
{
    public static List<JobNeedLevelRecord> CreateDefaults()
    {
        return
        [
            DefaultChemist.Clone(),
            DefaultKnight.Clone(),
            DefaultArcher.Clone(),
            DefaultMonk.Clone(),
            DefaultWhiteMage.Clone(),
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
            DefaultDarkKnight.Clone(),
            DefaultOnionKnight.Clone(),
            DefaultUnknown21.Clone(),
        ];
    }

    // Id 0: Chemist - no requirements
    private static readonly JobNeedLevelRecord DefaultChemist = new()
    {
        Id = 0,
        DisplayName = "Chemist",
    };

    // Id 1: Knight - Squire Lv2
    private static readonly JobNeedLevelRecord DefaultKnight = new()
    {
        Id = 1,
        DisplayName = "Knight",
        Squire = 2,
    };

    // Id 2: Archer - Squire Lv2
    private static readonly JobNeedLevelRecord DefaultArcher = new()
    {
        Id = 2,
        DisplayName = "Archer",
        Squire = 2,
    };

    // Id 3: Monk - Squire Lv2, Knight Lv3
    private static readonly JobNeedLevelRecord DefaultMonk = new()
    {
        Id = 3,
        DisplayName = "Monk",
        Squire = 2,
        Knight = 3,
    };

    // Id 4: White Mage - Chemist Lv2
    private static readonly JobNeedLevelRecord DefaultWhiteMage = new()
    {
        Id = 4,
        DisplayName = "White Mage",
        Chemist = 2,
    };

    // Id 5: Black Mage - Chemist Lv2
    private static readonly JobNeedLevelRecord DefaultBlackMage = new()
    {
        Id = 5,
        DisplayName = "Black Mage",
        Chemist = 2,
    };

    // Id 6: Time Mage - Chemist Lv2, Black Mage Lv3
    private static readonly JobNeedLevelRecord DefaultTimeMage = new()
    {
        Id = 6,
        DisplayName = "Time Mage",
        Chemist = 2,
        BlackMage = 3,
    };

    // Id 7: Summoner - Chemist Lv2, Black Mage Lv3, Time Mage Lv3
    private static readonly JobNeedLevelRecord DefaultSummoner = new()
    {
        Id = 7,
        DisplayName = "Summoner",
        Chemist = 2,
        BlackMage = 3,
        TimeMage = 3,
    };

    // Id 8: Thief - Squire Lv2, Archer Lv3
    private static readonly JobNeedLevelRecord DefaultThief = new()
    {
        Id = 8,
        DisplayName = "Thief",
        Squire = 2,
        Archer = 3,
    };

    // Id 9: Orator - Chemist Lv2, White Mage Lv3, Mystic Lv3
    private static readonly JobNeedLevelRecord DefaultOrator = new()
    {
        Id = 9,
        DisplayName = "Orator",
        Chemist = 2,
        WhiteMage = 3,
        Mystic = 3,
    };

    // Id 10: Mystic - Chemist Lv2, White Mage Lv3
    private static readonly JobNeedLevelRecord DefaultMystic = new()
    {
        Id = 10,
        DisplayName = "Mystic",
        Chemist = 2,
        WhiteMage = 3,
    };

    // Id 11: Geomancer - Squire Lv2, Knight Lv3, Monk Lv4
    private static readonly JobNeedLevelRecord DefaultGeomancer = new()
    {
        Id = 11,
        DisplayName = "Geomancer",
        Squire = 2,
        Knight = 3,
        Monk = 4,
    };

    // Id 12: Dragoon - Squire Lv2, Archer Lv3, Thief Lv4
    private static readonly JobNeedLevelRecord DefaultDragoon = new()
    {
        Id = 12,
        DisplayName = "Dragoon",
        Squire = 2,
        Archer = 3,
        Thief = 4,
    };

    // Id 13: Samurai - Squire Lv2, Knight Lv4, Archer Lv3, Monk Lv5, Thief Lv4, Dragoon Lv2
    private static readonly JobNeedLevelRecord DefaultSamurai = new()
    {
        Id = 13,
        DisplayName = "Samurai",
        Squire = 2,
        Knight = 4,
        Archer = 3,
        Monk = 5,
        Thief = 4,
        Dragoon = 2,
    };

    // Id 14: Ninja - Squire Lv2, Knight Lv3, Archer Lv4, Monk Lv4, Thief Lv5, Geomancer Lv2
    private static readonly JobNeedLevelRecord DefaultNinja = new()
    {
        Id = 14,
        DisplayName = "Ninja",
        Squire = 2,
        Knight = 3,
        Archer = 4,
        Monk = 4,
        Thief = 5,
        Geomancer = 2,
    };

    // Id 15: Arithmetician - Chemist Lv2, White Mage Lv5, Black Mage Lv5, Time Mage Lv4, Mystic Lv4
    private static readonly JobNeedLevelRecord DefaultArithmetician = new()
    {
        Id = 15,
        DisplayName = "Arithmetician",
        Chemist = 2,
        WhiteMage = 5,
        BlackMage = 5,
        TimeMage = 4,
        Mystic = 4,
    };

    // Id 16: Bard - Chemist Lv2, White Mage Lv3, Black Mage Lv3, Time Mage Lv3, Summoner Lv5, Orator Lv5, Mystic Lv3
    private static readonly JobNeedLevelRecord DefaultBard = new()
    {
        Id = 16,
        DisplayName = "Bard",
        Chemist = 2,
        WhiteMage = 3,
        BlackMage = 3,
        TimeMage = 3,
        Summoner = 5,
        Orator = 5,
        Mystic = 3,
    };

    // Id 17: Dancer - Squire Lv2, Knight Lv3, Archer Lv3, Monk Lv4, Thief Lv4, Geomancer Lv5, Dragoon Lv5
    private static readonly JobNeedLevelRecord DefaultDancer = new()
    {
        Id = 17,
        DisplayName = "Dancer",
        Squire = 2,
        Knight = 3,
        Archer = 3,
        Monk = 4,
        Thief = 4,
        Geomancer = 5,
        Dragoon = 5,
    };

    // Id 18: Mime - Squire Lv8, Chemist Lv8, Knight Lv3, Archer Lv3, Monk Lv4,
    //               White Mage Lv3, Black Mage Lv3, Time Mage Lv3, Summoner Lv5,
    //               Thief Lv4, Orator Lv5, Mystic Lv3, Geomancer Lv5, Dragoon Lv5
    private static readonly JobNeedLevelRecord DefaultMime = new()
    {
        Id = 18,
        DisplayName = "Mime",
        Squire = 8,
        Chemist = 8,
        Knight = 3,
        Archer = 3,
        Monk = 4,
        WhiteMage = 3,
        BlackMage = 3,
        TimeMage = 3,
        Summoner = 5,
        Thief = 4,
        Orator = 5,
        Mystic = 3,
        Geomancer = 5,
        Dragoon = 5,
    };

    // Id 19: Dark Knight - Geomancer Lv8, Dragoon Lv8, Samurai Lv8, Ninja Lv8
    private static readonly JobNeedLevelRecord DefaultDarkKnight = new()
    {
        Id = 19,
        DisplayName = "Dark Knight",
        Geomancer = 8,
        Dragoon = 8,
        Samurai = 8,
        Ninja = 8,
    };

    // Id 20: Onion Knight - Squire Lv6, Chemist Lv6
    private static readonly JobNeedLevelRecord DefaultOnionKnight = new()
    {
        Id = 20,
        DisplayName = "Onion Knight",
        Squire = 6,
        Chemist = 6,
    };

    // Id 21: Unknown - all zeros
    private static readonly JobNeedLevelRecord DefaultUnknown21 = new()
    {
        Id = 21,
        DisplayName = "Unknown21",
    };
}
