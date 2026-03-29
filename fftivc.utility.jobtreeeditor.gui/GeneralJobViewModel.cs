using fftivc.utility.jobtreeeditor.shared.GeneralJob;

namespace fftivc.utility.jobtreeeditor.gui;

public class GeneralJobViewModel(GeneralJobRecord record, GeneralJobRecord original)
{
    public GeneralJobRecord Record { get; } = record;
    private readonly GeneralJobRecord _original = original;

    public int Key => Record.Key;
    public string Name => Record.DisplayName;
    public string DisplayLabel => $"{Record.Key} - {Record.DisplayName}";
    public string RightDisplay => GeneralJobDefaults.FormatJobRef((int)Record.RightNeighbor);
    public string DownDisplay => GeneralJobDefaults.FormatJobRef((int)Record.DownNeighbor);
    public string LeftDisplay => GeneralJobDefaults.FormatJobRef((int)Record.LeftNeighbor);
    public string UpDisplay => GeneralJobDefaults.FormatJobRef((int)Record.UpNeighbor);

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
            if (Record.RightNeighbor != _original.RightNeighbor) return true;
            if (Record.DownNeighbor != _original.DownNeighbor) return true;
            if (Record.LeftNeighbor != _original.LeftNeighbor) return true;
            if (Record.UpNeighbor != _original.UpNeighbor) return true;
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

    /// <summary>Call after modifying Record to refresh bound properties.</summary>
    public void Refresh() { /* Grid.Items.Refresh() handles this for non-INotifyPropertyChanged */ }
}
