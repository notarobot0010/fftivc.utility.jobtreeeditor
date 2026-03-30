using fftivc.utility.jobtreeeditor.shared.Enums;
using fftivc.utility.jobtreeeditor.shared.GeneralJob;

namespace fftivc.utility.jobtreeeditor.shared;

/// <summary>
/// Calculates D-pad cursor neighbors and fallback neighbors based job swaps.
/// </summary>
public class NeighborCalculator
{
    /// <summary>
    /// After two jobs swap positions/bindings, update all neighbor references across all records.
    /// Every neighbor that pointed to jobA now points to jobB, and vice versa.
    /// Handles all 8 directional neighbors and all 8 gender fallback neighbors.
    /// </summary>
    public static void SwapNeighborReferences(List<GeneralJobRecord> records, GeneralJobKey jobA, GeneralJobKey jobB)
    {
        var recA = records.First(r => (GeneralJobKey)r.Key == jobA);
        var recB = records.First(r => (GeneralJobKey)r.Key == jobB);

        // Step 1: Swap the two jobs' own neighbor values
        // Step 1: Swap the two jobs' own neighbor values (all 16 fields)
        SwapRecordNeighbors(recA, recB);

        // Step 2: Across ALL records (including A and B), replace references
        // so anything pointing to A now points to B and vice versa
        foreach (var rec in records)
        {
            // Primary neighbors
            rec.RightNeighbor = SwapValue(rec.RightNeighbor, jobA, jobB);
            rec.DownRightNeighbor = SwapValue(rec.DownRightNeighbor, jobA, jobB);
            rec.DownNeighbor = SwapValue(rec.DownNeighbor, jobA, jobB);
            rec.DownLeftNeighbor = SwapValue(rec.DownLeftNeighbor, jobA, jobB);
            rec.LeftNeighbor = SwapValue(rec.LeftNeighbor, jobA, jobB);
            rec.UpLeftNeighbor = SwapValue(rec.UpLeftNeighbor, jobA, jobB);
            rec.UpNeighbor = SwapValue(rec.UpNeighbor, jobA, jobB);
            rec.UpRightNeighbor = SwapValue(rec.UpRightNeighbor, jobA, jobB);

            // Fallback neighbors
            rec.RightFallback = SwapValue(rec.RightFallback, jobA, jobB);
            rec.DownRightFallback = SwapValue(rec.DownRightFallback, jobA, jobB);
            rec.DownFallback = SwapValue(rec.DownFallback, jobA, jobB);
            rec.DownLeftFallback = SwapValue(rec.DownLeftFallback, jobA, jobB);
            rec.LeftFallback = SwapValue(rec.LeftFallback, jobA, jobB);
            rec.UpLeftFallback = SwapValue(rec.UpLeftFallback, jobA, jobB);
            rec.UpFallback = SwapValue(rec.UpFallback, jobA, jobB);
            rec.UpRightFallback = SwapValue(rec.UpRightFallback, jobA, jobB);
        }
    }

    private static void SwapRecordNeighbors(GeneralJobRecord recA, GeneralJobRecord recB)
    {
        // Primary neighbors
        (recA.RightNeighbor, recB.RightNeighbor) = (recB.RightNeighbor, recA.RightNeighbor);
        (recA.DownRightNeighbor, recB.DownRightNeighbor) = (recB.DownRightNeighbor, recA.DownRightNeighbor);
        (recA.DownNeighbor, recB.DownNeighbor) = (recB.DownNeighbor, recA.DownNeighbor);
        (recA.DownLeftNeighbor, recB.DownLeftNeighbor) = (recB.DownLeftNeighbor, recA.DownLeftNeighbor);
        (recA.LeftNeighbor, recB.LeftNeighbor) = (recB.LeftNeighbor, recA.LeftNeighbor);
        (recA.UpLeftNeighbor, recB.UpLeftNeighbor) = (recB.UpLeftNeighbor, recA.UpLeftNeighbor);
        (recA.UpNeighbor, recB.UpNeighbor) = (recB.UpNeighbor, recA.UpNeighbor);
        (recA.UpRightNeighbor, recB.UpRightNeighbor) = (recB.UpRightNeighbor, recA.UpRightNeighbor);

        // Fallback neighbors
        (recA.RightFallback, recB.RightFallback) = (recB.RightFallback, recA.RightFallback);
        (recA.DownRightFallback, recB.DownRightFallback) = (recB.DownRightFallback, recA.DownRightFallback);
        (recA.DownFallback, recB.DownFallback) = (recB.DownFallback, recA.DownFallback);
        (recA.DownLeftFallback, recB.DownLeftFallback) = (recB.DownLeftFallback, recA.DownLeftFallback);
        (recA.LeftFallback, recB.LeftFallback) = (recB.LeftFallback, recA.LeftFallback);
        (recA.UpLeftFallback, recB.UpLeftFallback) = (recB.UpLeftFallback, recA.UpLeftFallback);
        (recA.UpFallback, recB.UpFallback) = (recB.UpFallback, recA.UpFallback);
        (recA.UpRightFallback, recB.UpRightFallback) = (recB.UpRightFallback, recA.UpRightFallback);
    }

    private static GeneralJobKey SwapValue(GeneralJobKey current, GeneralJobKey a, GeneralJobKey b)
    {
        if (current == a) return b;
        if (current == b) return a;
        return current;
    }
}
