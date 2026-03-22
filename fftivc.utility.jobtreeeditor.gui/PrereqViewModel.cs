using fftivc.utility.jobtreeeditor.generaljob;

namespace fftivc.utility.jobtreeeditor.gui;

public class PrereqViewModel(JobPrerequisite prereq)
{
    private readonly JobPrerequisite _prereq = prereq;

    public string RequiredJobDisplay => GeneralJobDefaults.FormatJobRef((int)_prereq.RequiredJobId);
    public int RequiredLevel
    {
        get => _prereq.RequiredLevel;
        set => _prereq.RequiredLevel = value;
    }
    public string PositionDisplay => _prereq.LevelRequirementPosition.ToString();

    public JobPrerequisite ToPrerequisite() => _prereq.Clone();
}
