namespace fftivc.utility.jobtreeeditor.uib;

/// <summary>
/// Constants derived from reverse-engineering the ffto_job_tree.uib file.
/// </summary>
public class UibConstants
{
    // <summary>Expected magic bytes at file offset 0.</summary>
    public static readonly byte[] Magic = { 0x55, 0x49, 0x42, 0x00 }; // "UIB\0"

    /// <summary>Base address of record 0 (Squire) in the UIB file.</summary>
    public const int BaseAddress = 0x23BBC;

    /// <summary>Stride between consecutive job records in bytes.</summary>
    public const int RecordStride = 0x17C; // 380 bytes

    /// <summary>Offset from record base to the X position field.</summary>
    public const int XOffset = 0x170;

    /// <summary>Offset from record base to the Y position field.</summary>
    public const int YOffset = 0x174;

    /// <summary>Coordinate space width.</summary>
    public const int ScreenWidth = 1920;

    /// <summary>Coordinate space height.</summary>
    public const int ScreenHeight = 1080;

    /// <summary>
    /// All 20 job slots in UIB widget order.
    /// RecordIndex, Name, GeneralJobKey, DefaultX, DefaultY
    /// </summary>
    public static readonly JobSlot[] Jobs =
    {
        new( 0, "Squire",        0,   404,   90),
        new( 1, "Archer",        3,   200,  280),
        new( 2, "Knight",        2,   608,  280),
        new( 3, "Thief",         9,   200,  600),
        new( 4, "Ninja",        15,   404,  600),
        new( 5, "Monk",          4,   608,  480),
        new( 6, "Geomancer",    12,   812,  600),
        new( 7, "Dragoon",      13,   404,  850),
        new( 8, "Dancer",       18,   608,  730),
        new( 9, "Samurai",      14,   812,  848),
        new(10, "Chemist",       1,  1424,   89),
        new(11, "White Mage",    5,  1220,  280),
        new(12, "Black Mage",    6,  1628,  280),
        new(13, "Mystic",       11,  1220,  485),
        new(14, "Arithmetician",16,  1424,  485),
        new(15, "Time Mage",     7,  1628,  485),
        new(16, "Orator",       10,  1220,  690),
        new(17, "Bard",         17,  1424,  690),
        new(18, "Summoner",      8,  1628,  690),
        new(19, "Mime",         19,  1016,  849),
    };
}
