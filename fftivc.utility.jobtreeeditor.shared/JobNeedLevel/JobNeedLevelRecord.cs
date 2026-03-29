namespace fftivc.utility.jobtreeeditor.shared.JobNeedLevel;

/// <summary>
/// A single entry in the JobNeedLevelData.
/// Each record defines the job levels required to unlock a specific job.
/// The Id corresponds to <see cref="Enums.JobNeedLevelId"/>.
/// </summary>
public class JobNeedLevelRecord
{
    public int Id { get; set; } = 0;
    public string DisplayName { get; set; } = "";

    // Level required in each job to unlock this job (0 = not required)
    public int Squire { get; set; } = 0;
    public int Chemist { get; set; } = 0;
    public int Knight { get; set; } = 0;
    public int Archer { get; set; } = 0;
    public int Monk { get; set; } = 0;
    public int WhiteMage { get; set; } = 0;
    public int BlackMage { get; set; } = 0;
    public int TimeMage { get; set; } = 0;
    public int Summoner { get; set; } = 0;
    public int Thief { get; set; } = 0;
    public int Orator { get; set; } = 0;
    public int Mystic { get; set; } = 0;
    public int Geomancer { get; set; } = 0;
    public int Dragoon { get; set; } = 0;
    public int Samurai { get; set; } = 0;
    public int Ninja { get; set; } = 0;
    public int Arithmetician { get; set; } = 0;
    public int Bard { get; set; } = 0;
    public int Dancer { get; set; } = 0;
    public int Mime { get; set; } = 0;
    public int DarkKnight { get; set; } = 0;
    public int OnionKnight { get; set; } = 0;
    public int Unknown1 { get; set; } = 0;
    public int Unknown2 { get; set; } = 0;

    public JobNeedLevelRecord Clone()
    {
        return new JobNeedLevelRecord
        {
            Id = Id,
            DisplayName = DisplayName,
            Squire = Squire,
            Chemist = Chemist,
            Knight = Knight,
            Archer = Archer,
            Monk = Monk,
            WhiteMage = WhiteMage,
            BlackMage = BlackMage,
            TimeMage = TimeMage,
            Summoner = Summoner,
            Thief = Thief,
            Orator = Orator,
            Mystic = Mystic,
            Geomancer = Geomancer,
            Dragoon = Dragoon,
            Samurai = Samurai,
            Ninja = Ninja,
            Arithmetician = Arithmetician,
            Bard = Bard,
            Dancer = Dancer,
            Mime = Mime,
            DarkKnight = DarkKnight,
            OnionKnight = OnionKnight,
            Unknown1 = Unknown1,
            Unknown2 = Unknown2,
        };
    }
}
