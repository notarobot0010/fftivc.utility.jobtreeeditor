using fftivc.utility.jobtreeeditor.generaljob.Enums;

namespace fftivc.utility.jobtreeeditor.generaljob;

/// <summary>
/// Complete GeneralJob record for one job class.
/// </summary>
public class GeneralJobRecord
{
    /// <summary>Primary key in the GeneralJob table.</summary>
    public int Key { get; set; }

    /// <summary>Human-readable job name (not stored in NXD, just for our UI).</summary>
    public string Name { get; set; } = "";

    /// <summary>Comment column</summary>
    public string Comment { get; set; } = "";

    /// <summary>Must always contain exactly 9 values. Index 0 is base (level 0), indices 1-8 are JP thresholds for job levels 1-8.</summary>
    public const int RequiredExpLevels = 9;

    /// <summary>Experience thresholds per level. Default is: [0,100,200,400,700,1100,1600,2200,3000].</summary>
    public List<int> RequiredJobExp { get; set; } = [];

    /// <summary>Prerequisite sets (parallel arrays zipped together).</summary>
    public List<JobPrerequisite> Prerequisites { get; set; } = [];

    /// <summary>D-pad right neighbor (20/None = no neighbor).</summary>
    public Job RightNeighbor { get; set; }

    /// <summary>D-pad down neighbor (20/None = no neighbor).</summary>
    public Job DownNeighbor { get; set; }

    /// <summary>D-pad left neighbor (20/None = no neighbor).</summary>
    public Job LeftNeighbor { get; set; }

    /// <summary>D-pad up neighbor (20/None = no neighbor).</summary>
    public Job UpNeighbor { get; set; }

    /// <summary>Value meaning "no neighbor in this direction".</summary>
    public const Job NoNeighbor = Job.None;

    public GeneralJobRecord Clone()
    {
        return new GeneralJobRecord
        {
            Key = Key,
            Name = Name,
            Comment = Comment,
            RequiredJobExp = [.. RequiredJobExp],
            Prerequisites = [.. Prerequisites.Select(p => p.Clone())],
            RightNeighbor = RightNeighbor,
            DownNeighbor = DownNeighbor,
            LeftNeighbor = LeftNeighbor,
            UpNeighbor = UpNeighbor,
        };
    }
}
