namespace fftivc.utility.jobtreeeditor.uib;

/// <summary>
/// Handles reading and writing job positions in the UIB binary file.
/// </summary>
public class JobTreeUib
{
    private byte[] _data;
    public string SourcePath { get; }

    public JobTreeUib(string path)
    {
        SourcePath = path;
        _data = File.ReadAllBytes(path);
        Validate();
    }

    /// <summary>
    /// Create from raw bytes (e.g. for testing).
    /// </summary>
    public JobTreeUib(byte[] data, string sourcePath = "<memory>")
    {
        _data = (byte[])data.Clone();
        SourcePath = sourcePath;
        Validate();
    }

    private void Validate()
    {
        if (_data.Length < UibConstants.BaseAddress + (20 * UibConstants.RecordStride))
            throw new InvalidDataException(
                $"File is too small ({_data.Length} bytes). Expected at least " +
                $"{UibConstants.BaseAddress + (20 * UibConstants.RecordStride)} bytes for a valid job tree UIB.");

        for (int i = 0; i < UibConstants.Magic.Length; i++)
        {
            if (_data[i] != UibConstants.Magic[i])
                throw new InvalidDataException(
                    "File does not start with UIB magic bytes. Is this the correct file?");
        }
    }

    /// <summary>
    /// Read the current position of a job from the binary data.
    /// </summary>
    public JobPosition ReadPosition(JobSlot slot)
    {
        int x = BitConverter.ToInt32(_data, slot.XAddress);
        int y = BitConverter.ToInt32(_data, slot.YAddress);
        return new JobPosition(x, y);
    }

    /// <summary>
    /// Read all 20 job positions.
    /// </summary>
    public Dictionary<JobSlot, JobPosition> ReadAllPositions()
    {
        var result = new Dictionary<JobSlot, JobPosition>();
        foreach (var slot in UibConstants.Jobs)
            result[slot] = ReadPosition(slot);
        return result;
    }

    /// <summary>
    /// Write a position for a single job.
    /// </summary>
    public void WritePosition(JobSlot slot, JobPosition pos)
    {
        byte[] xBytes = BitConverter.GetBytes(pos.X);
        byte[] yBytes = BitConverter.GetBytes(pos.Y);
        Array.Copy(xBytes, 0, _data, slot.XAddress, 4);
        Array.Copy(yBytes, 0, _data, slot.YAddress, 4);
    }

    /// <summary>
    /// Swap the positions of two jobs (bi-directional).
    /// </summary>
    public void SwapPositions(JobSlot a, JobSlot b)
    {
        var posA = ReadPosition(a);
        var posB = ReadPosition(b);
        WritePosition(a, posB);
        WritePosition(b, posA);
    }

    /// <summary>
    /// Reset a job to its default position.
    /// </summary>
    public void ResetToDefault(JobSlot slot)
    {
        WritePosition(slot, new JobPosition(slot.DefaultX, slot.DefaultY));
    }

    /// <summary>
    /// Reset all jobs to their default positions.
    /// </summary>
    public void ResetAllToDefaults()
    {
        foreach (var slot in UibConstants.Jobs)
            ResetToDefault(slot);
    }

    /// <summary>
    /// Save the modified data to a file, creating parent directories if needed.
    /// </summary>
    public void Save(string outputPath)
    {
        string? dir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        File.WriteAllBytes(outputPath, _data);
    }

    /// <summary>
    /// Get a copy of the raw file data.
    /// </summary>
    public byte[] GetData() => (byte[])_data.Clone();
}
