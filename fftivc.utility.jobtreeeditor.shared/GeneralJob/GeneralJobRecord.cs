using fftivc.utility.jobtreeeditor.shared.Enums;

namespace fftivc.utility.jobtreeeditor.shared.GeneralJob;

/// <summary>
/// Complete GeneralJob record for one job class.
/// </summary>
public class GeneralJobRecord
{
    /// <summary>Primary key in the GeneralJob table.</summary>
    public int Key { get; set; }

    /// <summary>Human-readable job name (not stored in NXD, just for our UI).</summary>
    public string DisplayName { get; set; } = "";

    /// <summary>Comment column</summary>
    public string Comment { get; set; } = "";

    /// <summary>Must always contain exactly 9 values. Index 0 is base (level 0), indices 1-8 are JP thresholds for job levels 1-8.</summary>
    public const int RequiredExpLevels = 9;

    /// <summary>Experience thresholds per level. Default is: [0,100,200,400,700,1100,1600,2200,3000].</summary>
    public List<int> RequiredJobExp { get; set; } = [];

    /// <summary>Prerequisite sets (parallel arrays: Job, Level, RequirementPosition).</summary>
    public List<JobPrerequisite> Prerequisites { get; set; } = [];

    /// <summary>D-pad right neighbor (20/None = no neighbor).</summary>
    public Job RightNeighbor { get; set; }

    /// <summary>D-pad right fallback if neighbor is not available (20/None = no neighbor).</summary>
    public Job RightFallback { get; set; }

    ///  <summary>D-pad down-right neighbor (20/None = no neighbor).</summary>
    public Job DownRightNeighbor { get; set; }

    /// <summary>D-pad down-right fallback if neighbor is not available (20/None = no neighbor).</summary>
    public Job DownRightFallback { get; set; }

    /// <summary>D-pad down neighbor (20/None = no neighbor).</summary>
    public Job DownNeighbor { get; set; }

    /// <summary>D-pad down fallback if neighbor is not available (20/None = no neighbor).</summary>
    public Job DownFallback { get; set; }

    ///  <summary>D-pad down-left neighbor (20/None = no neighbor).</summary>
    public Job DownLeftNeighbor { get; set; }

    /// <summary>D-pad down-left fallback if neighbor is not available (20/None = no neighbor).</summary>
    public Job DownLeftFallback { get; set; }

    /// <summary>D-pad left neighbor (20/None = no neighbor).</summary>
    public Job LeftNeighbor { get; set; }

    /// <summary>D-pad left fallback if neighbor is not available (20/None = no neighbor).</summary>
    public Job LeftFallback { get; set; }

    ///  <summary>D-pad up-left neighbor (20/None = no neighbor).</summary>
    public Job UpLeftNeighbor { get; set; }

    /// <summary>D-pad up-left fallback if neighbor is not available (20/None = no neighbor).</summary>
    public Job UpLeftFallback { get; set; }

    /// <summary>D-pad up neighbor (20/None = no neighbor).</summary>
    public Job UpNeighbor { get; set; }

    /// <summary>D-pad up fallback if neighbor is not available (20/None = no neighbor).</summary>
    public Job UpFallback { get; set; }

    ///  <summary>D-pad up-right neighbor (20/None = no neighbor).</summary>
    public Job UpRightNeighbor { get; set; }

    /// <summary>D-pad up-right fallback if neighbor is not available (20/None = no neighbor).</summary>
    public Job UpRightFallback { get; set; }

    /// <summary>Value meaning "no neighbor in this direction".</summary>
    public const Job NoNeighbor = Job.None;

    public GeneralJobRecord Clone()
    {
        return new GeneralJobRecord
        {
            Key = Key,
            DisplayName = DisplayName,
            Comment = Comment,
            RequiredJobExp = [.. RequiredJobExp],
            Prerequisites = [.. Prerequisites.Select(p => p.Clone())],
            RightNeighbor = RightNeighbor,
            RightFallback = RightFallback,
            DownRightNeighbor = DownRightNeighbor,
            DownRightFallback = DownRightFallback,
            DownNeighbor = DownNeighbor,
            DownFallback = DownFallback,
            DownLeftNeighbor = DownLeftNeighbor,
            DownLeftFallback = DownLeftFallback,
            LeftNeighbor = LeftNeighbor,
            LeftFallback = LeftFallback,
            UpLeftNeighbor = UpLeftNeighbor,
            UpLeftFallback = UpLeftFallback,
            UpNeighbor = UpNeighbor,
            UpFallback = UpFallback,
            UpRightNeighbor = UpRightNeighbor,
            UpRightFallback = UpRightFallback,
        };
    }
}
