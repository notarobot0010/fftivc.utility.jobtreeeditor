using fftivc.utility.jobtreeeditor.shared;
using fftivc.utility.jobtreeeditor.shared.Enums;
using fftivc.utility.jobtreeeditor.shared.GeneralJob;
using System.ComponentModel;

namespace fftivc.utility.jobtreeeditor.gui;

public class PrereqViewModel(JobPrerequisite prereq) : INotifyPropertyChanged
{
    private readonly JobPrerequisite _prereq = prereq;

    // Display properties for view mode
    public string RequiredJobDisplay => GeneralJobDefaults.FormatJobRef((int)_prereq.RequiredJobId);
    public string PositionDisplay => _prereq.LevelRequirementPosition.ToString();

    // Editable properties for edit mode
    public int RequiredJobIndex
    {
        get => (int)_prereq.RequiredJobId;
        set
        {
            _prereq.RequiredJobId = (Job)value;
            OnPropertyChanged(nameof(RequiredJobIndex));
            OnPropertyChanged(nameof(RequiredJobDisplay));
        }
    }

    public int RequiredLevel
    {
        get => _prereq.RequiredLevel;
        set
        {
            _prereq.RequiredLevel = value;
            OnPropertyChanged(nameof(RequiredLevel));
        }
    }

    public int PositionIndex
    {
        get => (int)_prereq.LevelRequirementPosition - 1; // Enum starts at 1 (Left=1)
        set
        {
            _prereq.LevelRequirementPosition = (LevelRequirementPosition)(value + 1);
            OnPropertyChanged(nameof(PositionIndex));
            OnPropertyChanged(nameof(PositionDisplay));
        }
    }

    // Option lists for the dropdowns
    public List<string> JobOptions { get; } = [.. Enumerable.Range(0, 20).Select(i => GeneralJobDefaults.FormatJobRef(i))];

    public List<string> PositionOptions { get; } = [.. Enum.GetValues<LevelRequirementPosition>().Select(p => p.ToString())];

    public JobPrerequisite ToPrerequisite() => _prereq.Clone();

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
