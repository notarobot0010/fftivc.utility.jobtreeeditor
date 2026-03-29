namespace fftivc.utility.jobtreeeditor.uib;

/// <summary>
/// The name binding for a UIB position record, identifying which job the game
/// displays at that slot. Analogous to <see cref="JobPosition"/> for X/Y.
/// </summary>
/// <remarks>
/// <see cref="Value"/> is an <b>absolute file address</b> pointing 4 bytes into
/// the target job-name string in the widget name table.
/// <see cref="JobTreeUib.ReadNameRef"/> converts the on-disk relative offset to
/// this absolute form, and <see cref="JobTreeUib.WriteNameRef"/> converts back,
/// so the same <c>JobNameRef</c> can be read from one slot and written to any
/// other slot without manual recalculation.
/// </remarks>
public class JobNameRef(int value)
{
    /// <summary>
    /// Absolute file address of the target job-name string (landing 4 bytes in).
    /// </summary>
    public int Value { get; set; } = value;

    public JobNameRef Clone() => new(Value);

    // Hexadecimal format with padding 5 leading zeros
    public override string ToString() => $"NameRef(0x{Value:X5})";
}
