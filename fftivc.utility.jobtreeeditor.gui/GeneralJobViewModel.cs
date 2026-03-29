using fftivc.utility.jobtreeeditor.shared.GeneralJob;

namespace fftivc.utility.jobtreeeditor.gui;

public class GeneralJobViewModel(GeneralJobRecord record, GeneralJobRecord original)
{
    public GeneralJobRecord Record { get; } = record;
    private readonly GeneralJobRecord _original = original;

    public int Key => Record.Key;
    public string Name => Record.DisplayName;
    public string DisplayLabel => $"{Record.Key} - {Record.DisplayName}";
    public string RightNeighborDisplay => GeneralJobDefaults.FormatJobRef((int)Record.RightNeighbor);
    public string DownRightNeighborDisplay => GeneralJobDefaults.FormatJobRef((int)Record.DownRightNeighbor);
    public string DownNeighborDisplay => GeneralJobDefaults.FormatJobRef((int)Record.DownNeighbor);
    public string DownLeftNeighborDisplay => GeneralJobDefaults.FormatJobRef((int)Record.DownLeftNeighbor);
    public string LeftNeighborDisplay => GeneralJobDefaults.FormatJobRef((int)Record.LeftNeighbor);
    public string UpLeftNeighborDisplay => GeneralJobDefaults.FormatJobRef((int)Record.UpLeftNeighbor);
    public string UpNeighborDisplay => GeneralJobDefaults.FormatJobRef((int)Record.UpNeighbor);
    public string UpRightNeighborDisplay => GeneralJobDefaults.FormatJobRef((int)Record.UpRightNeighbor);

    public string RightFallbackDisplay => GeneralJobDefaults.FormatJobRef((int)Record.RightFallback);
    public string DownRightFallbackDisplay => GeneralJobDefaults.FormatJobRef((int)Record.DownRightFallback);
    public string DownFallbackDisplay => GeneralJobDefaults.FormatJobRef((int)Record.DownFallback);
    public string DownLeftFallbackDisplay => GeneralJobDefaults.FormatJobRef((int)Record.DownLeftFallback);
    public string LeftFallbackDisplay => GeneralJobDefaults.FormatJobRef((int)Record.LeftFallback);
    public string UpLeftFallbackDisplay => GeneralJobDefaults.FormatJobRef((int)Record.UpLeftFallback);
    public string UpFallbackDisplay => GeneralJobDefaults.FormatJobRef((int)Record.UpFallback);
    public string UpRightFallbackDisplay => GeneralJobDefaults.FormatJobRef((int)Record.UpRightFallback);

    public string PrereqSummary
    {
        get
        {
            if (Record.Prerequisites.Count == 0) return "(none)";
            return string.Join(", ", Record.Prerequisites.Select(p =>
                $"{GeneralJobDefaults.JobNames.GetValueOrDefault((int)p.RequiredJobId, "?")} Lv{p.RequiredLevel}"));
        }
    }

    public bool IsModified
    {
        get
        {
            if (IsAnyNeighborModified) return true;
            if (Record.Comment != _original.Comment) return true;
            if (Record.Prerequisites.Count != _original.Prerequisites.Count) return true;
            for (int i = 0; i < Record.Prerequisites.Count; i++)
            {
                var c = Record.Prerequisites[i];
                var o = _original.Prerequisites[i];
                if (c.RequiredJobId != o.RequiredJobId || c.RequiredLevel != o.RequiredLevel
                    || c.LevelRequirementPosition != o.LevelRequirementPosition) return true;
            }
            return false;
        }
    }

    private bool IsAnyNeighborModified
    {
        get 
        {
            if (Record.RightNeighbor != _original.RightNeighbor) return true;
            if (Record.RightFallback != _original.RightFallback) return true;

            if (Record.DownRightNeighbor != _original.DownRightNeighbor) return true;
            if (Record.DownRightFallback != _original.DownRightFallback) return true;

            if (Record.DownNeighbor != _original.DownNeighbor) return true;
            if (Record.DownFallback != _original.DownFallback) return true;

            if (Record.DownLeftNeighbor != _original.DownLeftNeighbor) return true;
            if (Record.DownLeftFallback != _original.DownLeftFallback) return true;
            
            if (Record.LeftNeighbor != _original.LeftNeighbor) return true;
            if (Record.LeftFallback != _original.LeftFallback) return true;

            if (Record.UpLeftNeighbor != _original.UpLeftNeighbor) return true;
            if (Record.UpLeftFallback != _original.UpLeftFallback) return true;

            if (Record.UpNeighbor != _original.UpNeighbor) return true;
            if (Record.UpFallback != _original.UpFallback) return true;

            if (Record.UpRightNeighbor != _original.UpRightNeighbor) return true;
            if (Record.UpRightFallback != _original.UpRightFallback) return true;

            return false;
        }
    }

    /// <summary>Call after modifying Record to refresh bound properties.</summary>
    public void Refresh() { /* Grid.Items.Refresh() handles this for non-INotifyPropertyChanged */ }
}
