namespace fftivc.utility.jobtreeeditor.uib;

/// <summary>
/// Constants derived from reverse-engineering the ffto_job_tree.uib file.
/// </summary>
public class UibConstants
{
    // <summary>Expected magic bytes at file offset 0.</summary>
    public static readonly byte[] Magic = [0x55, 0x49, 0x42, 0x00]; // "UIB\0"

    /// <summary>Base address of record 0 (Squire) in the UIB file.</summary>
    public const int BaseAddress = 0x23BBC;

    /// <summary>Stride between consecutive job records in bytes.</summary>
    public const int RecordStride = 0x17C; // 380 bytes

    /// <summary>Offset from record base to the X position field.</summary>
    public const int XOffset = 0x170;

    /// <summary>Offset from record base to the Y position field.</summary>
    public const int YOffset = 0x174;

    /// <summary>
    /// Offset from record base to the name reference field.
    /// On disk this is a 4-byte little-endian relative offset. The game resolves it as:
    ///   string_addr = (record_base + NameRefOffset) + value
    /// The pointer lands 4 bytes into the target string; the game backs up
    /// to the preceding null terminator to read the full name.
    /// <see cref="JobTreeUib.ReadNameRef"/> and <see cref="JobTreeUib.WriteNameRef"/>
    /// handle the relative ↔ absolute conversion automatically.
    /// </summary>
    public const int NameRefOffset = 0x16C;

    // TODO: Remove
    /// <summary>
    /// The name reference pointer always lands this many bytes into the string.
    /// Used when computing a new reference value for a target string address:
    ///   value = (string_table_addr + NameRefLandingOffset) - (record_base + NameRefOffset)
    /// </summary>
    //public const int NameRefLandingOffset = 4;

    /// <summary>Coordinate space width.</summary>
    public const int ScreenWidth = 1920;

    /// <summary>Coordinate space height.</summary>
    public const int ScreenHeight = 1080;

    /// <summary>
    /// All 20 job slots in UIB widget order.
    /// RecordIndex, Name, GeneralJobKey, DefaultX, DefaultY, DefaultNameRef
    ///
    /// DefaultNameRef is the absolute file address of the job-name string
    /// (4 bytes into the string) in the unmodified UIB.
    /// </summary>
    public static readonly JobSlot[] Jobs =
    [
        new( 0, "Squire",         0,   404,   90, 0x2EA5B),  // -> "Squire"
        new( 1, "Archer",         3,   200,  280, 0x2EA62),  // -> "Archer"
        new( 2, "Knight",         2,   608,  280, 0x2EA69),  // -> "Knight"
        new( 3, "Thief",          9,   200,  600, 0x2EA70),  // -> "Thief"
        new( 4, "Ninja",         15,   404,  600, 0x2EA76),  // -> "Ninja"
        new( 5, "Monk",           4,   608,  480, 0x2EA7C),  // -> "Monk"
        new( 6, "Geomancer",     12,   812,  600, 0x2EA81),  // -> "Geomancer"
        new( 7, "Dragoon",       13,   404,  850, 0x2EA8B),  // -> "Dragoon"
        new( 8, "Dancer",        18,   608,  730, 0x2EA93),  // -> "Dancer"
        new( 9, "Samurai",       14,   812,  848, 0x2EA9A),  // -> "Samurai"
        new(10, "Chemist",        1,  1424,   89, 0x2EAA2),  // -> "Chemist"
        new(11, "White Mage",     5,  1220,  280, 0x2EAAA),  // -> "White"
        new(12, "Black Mage",     6,  1628,  280, 0x2EAB0),  // -> "Black"
        new(13, "Mystic",        11,  1220,  485, 0x2EAB6),  // -> "Mystic"
        new(14, "Arithmetician", 16,  1424,  485, 0x2EABD),  // -> "Arithmetician"
        new(15, "Time Mage",      7,  1628,  485, 0x2EACB),  // -> "Time"
        new(16, "Orator",        10,  1220,  690, 0x2EAD0),  // -> "Orator"
        new(17, "Bard",          17,  1424,  690, 0x2EAD7),  // -> "Bard"
        new(18, "Summoner",       8,  1628,  690, 0x2EADC),  // -> "Summoner"
        new(19, "Mime",          19,  1016,  849, 0x2EAE5),  // -> "Mime"
    ];

    public enum UibSlotName
    {
        Squire = 0, 
        Archer = 1, 
        Knight = 2, 
        Thief = 3, 
        Ninja = 4, 
        Monk = 5,
        Geomancer = 6, 
        Dragoon = 7, 
        Dancer = 8, 
        Samurai = 9, 
        Chemist = 10,
        White = 11, 
        Black = 12, 
        Mystic = 13, 
        Arithmetician = 14, 
        Time = 15,
        Orator = 16, 
        Bard = 17, 
        Summoner = 18, 
        Mime = 19,
    }

    public static readonly Dictionary<UibSlotName, int> SlotNameToAddress = new()
    {
        { UibSlotName.Squire,        0x2EA5B },
        { UibSlotName.Archer,        0x2EA62 },
        { UibSlotName.Knight,        0x2EA69 },
        { UibSlotName.Thief,         0x2EA70 },
        { UibSlotName.Ninja,         0x2EA76 },
        { UibSlotName.Monk,          0x2EA7C },
        { UibSlotName.Geomancer,     0x2EA81 },
        { UibSlotName.Dragoon,       0x2EA8B },
        { UibSlotName.Dancer,        0x2EA93 },
        { UibSlotName.Samurai,       0x2EA9A },
        { UibSlotName.Chemist,       0x2EAA2 },
        { UibSlotName.White,         0x2EAAA },
        { UibSlotName.Black,         0x2EAB0 },
        { UibSlotName.Mystic,        0x2EAB6 },
        { UibSlotName.Arithmetician, 0x2EABD },
        { UibSlotName.Time,          0x2EACB },
        { UibSlotName.Orator,        0x2EAD0 },
        { UibSlotName.Bard,          0x2EAD7 },
        { UibSlotName.Summoner,      0x2EADC },
        { UibSlotName.Mime,          0x2EAE5 },
    };

    public static readonly Dictionary<int, UibSlotName> AddressToSlotName =
        SlotNameToAddress.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
}
