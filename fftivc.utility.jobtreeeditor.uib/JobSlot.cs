using System;
using System.Collections.Generic;
using System.Text;

namespace fftivc.utility.jobtreeeditor.uib;

/// <summary>
/// Static definition of a job slot in the UIB file.
/// </summary>
public class JobSlot
{
    /// <summary>Index in the UIB widget order (0-19).</summary>
    public int RecordIndex { get; }

    /// <summary>Display name.</summary>
    public string Name { get; }

    /// <summary>Key in the GeneralJob NXD table.</summary>
    public int GeneralJobKey { get; }

    /// <summary>Default X position in the unmodified UIB.</summary>
    public int DefaultX { get; }

    /// <summary>Default Y position in the unmodified UIB.</summary>
    public int DefaultY { get; }

    /// <summary>Absolute file offset of the X value (4 bytes, little-endian int32).</summary>
    public int XAddress => UibConstants.BaseAddress + (RecordIndex * UibConstants.RecordStride) + UibConstants.XOffset;

    /// <summary>Absolute file offset of the Y value (4 bytes, little-endian int32).</summary>
    public int YAddress => XAddress + 4;

    public JobSlot(int recordIndex, string name, int generalJobKey, int defaultX, int defaultY)
    {
        RecordIndex = recordIndex;
        Name = name;
        GeneralJobKey = generalJobKey;
        DefaultX = defaultX;
        DefaultY = defaultY;
    }
}
