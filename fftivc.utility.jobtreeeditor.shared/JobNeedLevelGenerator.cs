using fftivc.utility.jobtreeeditor.shared.GeneralJob;
using fftivc.utility.jobtreeeditor.shared.JobNeedLevel;

namespace fftivc.utility.jobtreeeditor.shared;

/// <summary>
/// Generates the JobNeedLevelTable from GeneralJob prerequisite data.
/// 
/// The GeneralJob table stores only direct prerequisites (e.g., Monk requires Knight Lv3).
/// The NeedJobLevel table stores the full transitive closure: every job level a character
/// must have reached to unlock a given job (e.g., Monk requires Knight Lv3 AND Squire Lv2,
/// because Knight itself requires Squire Lv2).
/// 
/// When multiple paths lead to the same prerequisite job, the maximum level is used.
/// </summary>
public static class JobNeedLevelGenerator
{
    /// <summary>
    /// Maps GeneralJob Key to the column index used in <see cref="JobNeedLevelRecord"/>.
    /// The NeedJobLevel columns are ordered: Squire(0), Chemist(1), Knight(2), Archer(3), ...
    /// which differs from the GeneralJob Key ordering.
    /// </summary>
    private static readonly Dictionary<int, string> GeneralJobKeyToColumnName = new()
    {
        { 0, nameof(JobNeedLevelRecord.Squire) },
        { 1, nameof(JobNeedLevelRecord.Chemist) },
        { 2, nameof(JobNeedLevelRecord.Knight) },
        { 3, nameof(JobNeedLevelRecord.Archer) },
        { 4, nameof(JobNeedLevelRecord.Monk) },
        { 5, nameof(JobNeedLevelRecord.WhiteMage) },
        { 6, nameof(JobNeedLevelRecord.BlackMage) },
        { 7, nameof(JobNeedLevelRecord.TimeMage) },
        { 8, nameof(JobNeedLevelRecord.Summoner) },
        { 9, nameof(JobNeedLevelRecord.Thief) },
        { 10, nameof(JobNeedLevelRecord.Orator) },
        { 11, nameof(JobNeedLevelRecord.Mystic) },
        { 12, nameof(JobNeedLevelRecord.Geomancer) },
        { 13, nameof(JobNeedLevelRecord.Dragoon) },
        { 14, nameof(JobNeedLevelRecord.Samurai) },
        { 15, nameof(JobNeedLevelRecord.Ninja) },
        { 16, nameof(JobNeedLevelRecord.Arithmetician) },
        { 17, nameof(JobNeedLevelRecord.Bard) },
        { 18, nameof(JobNeedLevelRecord.Dancer) },
        { 19, nameof(JobNeedLevelRecord.Mime) },
    };

    /// <summary>
    /// Maps GeneralJob Key to JobNeedLevelId. Squire (Key 0) has no NeedJobLevel entry.
    /// </summary>
    private static readonly Dictionary<int, int> GeneralJobKeyToNeedLevelId = new()
    {
        // Squire (Key 0) is omitted — it's always available
        { 1, 0 },   // Chemist -> NeedLevelId 0
        { 2, 1 },   // Knight -> NeedLevelId 1
        { 3, 2 },   // Archer -> NeedLevelId 2
        { 4, 3 },   // Monk -> NeedLevelId 3
        { 5, 4 },   // White Mage -> NeedLevelId 4
        { 6, 5 },   // Black Mage -> NeedLevelId 5
        { 7, 6 },   // Time Mage -> NeedLevelId 6
        { 8, 7 },   // Summoner -> NeedLevelId 7
        { 9, 8 },   // Thief -> NeedLevelId 8
        { 10, 9 },  // Orator -> NeedLevelId 9
        { 11, 10 }, // Mystic -> NeedLevelId 10
        { 12, 11 }, // Geomancer -> NeedLevelId 11
        { 13, 12 }, // Dragoon -> NeedLevelId 12
        { 14, 13 }, // Samurai -> NeedLevelId 13
        { 15, 14 }, // Ninja -> NeedLevelId 14
        { 16, 15 }, // Arithmetician -> NeedLevelId 15
        { 17, 16 }, // Bard -> NeedLevelId 16
        { 18, 17 }, // Dancer -> NeedLevelId 17
        { 19, 18 }, // Mime -> NeedLevelId 18
    };

    /// <summary>
    /// Generate the full JobNeedLevel table from GeneralJob records.
    /// Produces entries for the 19 standard unlockable jobs (IDs 0–18).
    /// </summary>
    public static List<JobNeedLevelRecord> Generate(List<GeneralJobRecord> generalJobRecords)
    {
        var recordsByKey = generalJobRecords.ToDictionary(r => r.Key);
        var results = new List<JobNeedLevelRecord>();

        foreach (var rec in generalJobRecords)
        {
            // Squire (Key 0) has no NeedJobLevel entry
            if (!GeneralJobKeyToNeedLevelId.TryGetValue(rec.Key, out int needLevelId))
                continue;

            // Build the cumulative requirements by walking the prerequisite tree
            var cumulativeLevels = new Dictionary<int, int>(); // GeneralJob Key -> max level needed
            CollectRequirements(rec.Key, recordsByKey, cumulativeLevels);

            var record = new JobNeedLevelRecord
            {
                Id = needLevelId,
                DisplayName = rec.DisplayName,
            };

            // Set each column from the cumulative requirements
            foreach (var (jobKey, level) in cumulativeLevels)
            {
                SetColumn(record, jobKey, level);
            }

            results.Add(record);
        }

        // Sort by NeedLevelId
        results.Sort((a, b) => a.Id.CompareTo(b.Id));
        return results;
    }

    /// <summary>
    /// Recursively collect all prerequisite job levels needed to unlock a job.
    /// Uses the maximum level when multiple paths require the same prerequisite job.
    /// </summary>
    private static void CollectRequirements(
        int jobKey,
        Dictionary<int, GeneralJobRecord> recordsByKey,
        Dictionary<int, int> cumulativeLevels)
    {
        if (!recordsByKey.TryGetValue(jobKey, out var record))
            return;

        foreach (var prereq in record.Prerequisites)
        {
            int reqJobKey = (int)prereq.RequiredJobId;
            int reqLevel = prereq.RequiredLevel;

            // Take the maximum if this prerequisite job was already seen via another path
            if (cumulativeLevels.TryGetValue(reqJobKey, out int existing))
            {
                if (reqLevel <= existing)
                    continue; // Already have a higher or equal requirement, skip recursion
            }

            cumulativeLevels[reqJobKey] = Math.Max(reqLevel, existing);

            // Recurse into the prerequisite's own prerequisites
            CollectRequirements(reqJobKey, recordsByKey, cumulativeLevels);
        }
    }

    /// <summary>
    /// Set the appropriate column on a JobNeedLevelRecord by GeneralJob key.
    /// </summary>
    private static void SetColumn(JobNeedLevelRecord record, int generalJobKey, int level)
    {
        switch (generalJobKey)
        {
            case 0: record.Squire = level; break;
            case 1: record.Chemist = level; break;
            case 2: record.Knight = level; break;
            case 3: record.Archer = level; break;
            case 4: record.Monk = level; break;
            case 5: record.WhiteMage = level; break;
            case 6: record.BlackMage = level; break;
            case 7: record.TimeMage = level; break;
            case 8: record.Summoner = level; break;
            case 9: record.Thief = level; break;
            case 10: record.Orator = level; break;
            case 11: record.Mystic = level; break;
            case 12: record.Geomancer = level; break;
            case 13: record.Dragoon = level; break;
            case 14: record.Samurai = level; break;
            case 15: record.Ninja = level; break;
            case 16: record.Arithmetician = level; break;
            case 17: record.Bard = level; break;
            case 18: record.Dancer = level; break;
            case 19: record.Mime = level; break;
        }
    }
}

