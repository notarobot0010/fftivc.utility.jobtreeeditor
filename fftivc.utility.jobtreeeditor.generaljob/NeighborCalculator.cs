using fftivc.utility.jobtreeeditor.generaljob.Enums;
using fftivc.utility.jobtreeeditor.uib;
using System.IO.Compression;

namespace fftivc.utility.jobtreeeditor.generaljob;

/// <summary>
/// Calculates D-pad cursor neighbors based on spatial positions of jobs.
/// </summary>
public class NeighborCalculator
{
    /// <summary>
    /// For each job, find the nearest neighbor in each cardinal direction
    /// based on the current UIB positions.
    /// 
    /// The algorithm for each direction:
    /// - Right: among jobs with X > this job's X, find the one with the smallest
    ///   weighted distance, preferring jobs at a similar Y.
    /// - Left: among jobs with X &lt; this job's X, same logic.
    /// - Down: among jobs with Y > this job's Y, find nearest (Y increases downward).
    /// - Up: among jobs with Y &lt; this job's Y, find nearest.
    /// 
    /// If no job exists in a direction, the neighbor is set to Job.None (20).
    /// </summary>
    /// <param name="jobs">The GeneralJob records to update.</param>
    /// <param name="positions">Map of GeneralJob key → current (X, Y) position from the UIB.</param>
    public static void Calculate(List<GeneralJobRecord> jobs, Dictionary<int, JobPosition> positions)
    {
        foreach (var job in jobs)
        {
            if (!positions.TryGetValue(job.Key, out var myPos))
                continue;

            job.RightNeighbor = FindNearest(job.Key, myPos, jobs, positions, Direction.Right);
            job.LeftNeighbor = FindNearest(job.Key, myPos, jobs, positions, Direction.Left);
            job.DownNeighbor = FindNearest(job.Key, myPos, jobs, positions, Direction.Down);
            job.UpNeighbor = FindNearest(job.Key, myPos, jobs, positions, Direction.Up);
        }
    }

    private enum Direction { Right, Left, Down, Up }

    private static Job FindNearest(
        int myKey,
        JobPosition myPos,
        List<GeneralJobRecord> allJobs,
        Dictionary<int, JobPosition> positions,
        Direction dir)
    {
        Job bestJob = GeneralJobRecord.NoNeighbor;
        double bestScore = double.MaxValue;

        foreach (var other in allJobs)
        {
            if (other.Key == myKey) continue;
            if (!positions.TryGetValue(other.Key, out var otherPos)) continue;

            int dx = otherPos.X - myPos.X;
            int dy = otherPos.Y - myPos.Y;

            // Check if this job is in the correct direction.
            bool inDirection = dir switch
            {
                Direction.Right => dx > 0,
                Direction.Left => dx < 0,
                Direction.Down => dy > 0,
                Direction.Up => dy < 0,
                _ => false,
            };

            if (!inDirection) continue;

            // Score: prioritize the primary axis, penalize the secondary axis.
            // For horizontal directions (left/right), primary = |dx|, secondary = |dy|.
            // For vertical directions (up/down), primary = |dy|, secondary = |dx|.
            // We want the closest job in the primary direction, with a tie-breaking
            // preference for jobs that are aligned on the secondary axis.
            double primary, secondary;
            switch (dir)
            {
                case Direction.Right:
                case Direction.Left:
                    primary = Math.Abs(dx);
                    secondary = Math.Abs(dy);
                    break;
                default: // Up, Down
                    primary = Math.Abs(dy);
                    secondary = Math.Abs(dx);
                    break;
            }

            // Weighted score: secondary axis distance counts more heavily to avoid
            // selecting a job that's far off to the side. The weight of 2.0 means
            // a job 100px away on the primary axis but aligned is preferred over
            // one 80px away but 50px off to the side.
            double score = primary + (secondary * 2.0);

            if (score < bestScore)
            {
                bestScore = score;
                bestJob = (Job)other.Key;
            }
        }

        return bestJob;
    }

    /// <summary>
    /// Build a position lookup from UIB data, mapping GeneralJob key → position.
    /// Uses the UibConstants.Jobs array to map widget order → GeneralJob key.
    /// </summary>
    public static Dictionary<int, JobPosition> BuildPositionMap(JobTreeUib uib)
    {
        var map = new Dictionary<int, JobPosition>();
        foreach (var slot in UibConstants.Jobs)
        {
            var pos = uib.ReadPosition(slot);
            map[slot.GeneralJobKey] = pos;
        }
        return map;
    }
}
