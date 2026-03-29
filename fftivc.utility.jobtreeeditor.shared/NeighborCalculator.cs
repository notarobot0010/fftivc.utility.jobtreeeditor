using fftivc.utility.jobtreeeditor.shared.Enums;
using fftivc.utility.jobtreeeditor.shared.GeneralJob;

namespace fftivc.utility.jobtreeeditor.shared;

/// <summary>
/// Calculates D-pad cursor neighbors based job swaps.
/// </summary>
public class NeighborCalculator
{
    /// <summary>
    /// After two jobs swap positions, update all neighbor references across all records.
    /// Every neighbor that pointed to jobA now points to jobB, and vice versa.
    /// </summary>
    public static void SwapNeighborReferences(List<GeneralJobRecord> records, GeneralJobKey jobA, GeneralJobKey jobB)
    {
        var recA = records.First(r => (GeneralJobKey)r.Key == jobA);
        var recB = records.First(r => (GeneralJobKey)r.Key == jobB);

        // Step 1: Swap the two jobs' own neighbor values
        var tempRight = recA.RightNeighbor;
        var tempDown = recA.DownNeighbor;
        var tempLeft = recA.LeftNeighbor;
        var tempUp = recA.UpNeighbor;

        recA.RightNeighbor = recB.RightNeighbor;
        recA.DownNeighbor = recB.DownNeighbor;
        recA.LeftNeighbor = recB.LeftNeighbor;
        recA.UpNeighbor = recB.UpNeighbor;

        recB.RightNeighbor = tempRight;
        recB.DownNeighbor = tempDown;
        recB.LeftNeighbor = tempLeft;
        recB.UpNeighbor = tempUp;

        // Step 2: Across ALL records (including A and B), replace references
        // so anything pointing to A now points to B and vice versa
        foreach (var rec in records)
        {
            rec.RightNeighbor = SwapValue(rec.RightNeighbor, jobA, jobB);
            rec.DownNeighbor = SwapValue(rec.DownNeighbor, jobA, jobB);
            rec.LeftNeighbor = SwapValue(rec.LeftNeighbor, jobA, jobB);
            rec.UpNeighbor = SwapValue(rec.UpNeighbor, jobA, jobB);
        }
    }

    private static GeneralJobKey SwapValue(GeneralJobKey current, GeneralJobKey a, GeneralJobKey b)
    {
        if (current == a) return b;
        if (current == b) return a;
        return current;
    }
}
