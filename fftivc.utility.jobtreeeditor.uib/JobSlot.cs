namespace fftivc.utility.jobtreeeditor.uib;

/// <summary>
/// Static definition of a job slot in the UIB file.
/// </summary>
public class JobSlot(int recordIndex, string name, int generalJobKey, int defaultX, int defaultY, int defaultNameRef)
{
    /// <summary>Index in the UIB widget order (0-19).</summary>
    public int RecordIndex { get; } = recordIndex;

    /// <summary>Display name.</summary>
    public string Name { get; } = name;

    /// <summary>Key in the GeneralJob NXD table.</summary>
    public int GeneralJobKey { get; } = generalJobKey;

    /// <summary>Default X position in the unmodified UIB.</summary>
    public int DefaultX { get; } = defaultX;

    /// <summary>Default Y position in the unmodified UIB.</summary>
    public int DefaultY { get; } = defaultY;

    /// <summary>
    /// Default name reference in the unmodified UIB.
    /// This is an absolute file address pointing 4 bytes into the job-name string.
    /// </summary>
    public int DefaultNameRef { get; } = defaultNameRef;

    /// <summary>Absolute file offset of the record base.</summary>
    public int RecordBase => UibConstants.BaseAddress + (RecordIndex * UibConstants.RecordStride);

    /// <summary>Absolute file offset of the X value (4 bytes, little-endian int32).</summary>
    public int XAddress => RecordBase + UibConstants.XOffset;

    /// <summary>Absolute file offset of the Y value (4 bytes, little-endian int32).</summary>
    public int YAddress => RecordBase + UibConstants.YOffset;

    /// <summary>Absolute file offset of the name reference value (4 bytes, little-endian uint32).</summary>
    public int NameRefAddress => RecordBase + UibConstants.NameRefOffset;
}
