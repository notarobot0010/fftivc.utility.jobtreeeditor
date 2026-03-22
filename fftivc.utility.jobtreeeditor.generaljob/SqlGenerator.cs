using System.Text;

namespace fftivc.utility.jobtreeeditor.generaljob;

/// <summary>
/// Generates SQL UPDATE scripts for modified GeneralJob records.
/// </summary>
public static class SqlGenerator
{
    /// <summary>
    /// Compare the current records against their original state and generate
    /// UPDATE statements for any that changed.
    /// </summary>
    /// <param name="current">The current (potentially modified) records.</param>
    /// <param name="original">The original records to compare against.</param>
    /// <returns>A complete SQL script string, or empty if nothing changed.</returns>
    public static string Generate(List<GeneralJobRecord> current, List<GeneralJobRecord> original)
    {
        var sb = new StringBuilder();
        sb.AppendLine("-- FFT Ivalice Chronicles — GeneralJob edits");
        sb.AppendLine($"-- Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine();

        int changeCount = 0;

        foreach (var cur in current)
        {
            var orig = original.FirstOrDefault(o => o.Key == cur.Key);
            if (orig == null) continue;

            var setClauses = new List<string>();

            // Comment
            if (cur.Comment != orig.Comment)
                setClauses.Add($"Comment = '{EscapeSql(cur.Comment)}'");

            // RequiredJobExp
            if (!ListsEqual(cur.RequiredJobExp, orig.RequiredJobExp))
                setClauses.Add($"RequiredJobExp = '{FormatIntArray(cur.RequiredJobExp)}'");

            // Prerequisites (three parallel arrays)
            if (!PrerequisitesEqual(cur.Prerequisites, orig.Prerequisites))
            {
                var ids = cur.Prerequisites.Select(p => (int)p.RequiredJobId).ToList();
                var levels = cur.Prerequisites.Select(p => p.RequiredLevel).ToList();
                var positions = cur.Prerequisites.Select(p => (int)p.LevelRequirementPosition).ToList();

                setClauses.Add($"RequiredJobIds = '{FormatIntArray(ids)}'");
                setClauses.Add($"RequiredJobLevels = '{FormatIntArray(levels)}'");
                setClauses.Add($"RequiredJobPositions = '{FormatIntArray(positions)}'");
            }

            // Neighbors
            if (cur.RightNeighbor != orig.RightNeighbor)
                setClauses.Add($"Unknown28 = {(int)cur.RightNeighbor}");

            if (cur.DownNeighbor != orig.DownNeighbor)
                setClauses.Add($"Unknown30 = {(int)cur.DownNeighbor}");

            if (cur.LeftNeighbor != orig.LeftNeighbor)
                setClauses.Add($"Unknown38 = {(int)cur.LeftNeighbor}");

            if (cur.UpNeighbor != orig.UpNeighbor)
                setClauses.Add($"Unknown40 = {(int)cur.UpNeighbor}");

            if (setClauses.Count > 0)
            {
                string name = GeneralJobDefaults.JobNames.GetValueOrDefault(cur.Key, "???");
                sb.AppendLine($"-- {name} (Key={cur.Key})");
                sb.AppendLine($"UPDATE GeneralJob ");
                sb.AppendLine($"SET {string.Join(", ", setClauses)} ");
                sb.AppendLine($"WHERE Key = {cur.Key};");
                sb.AppendLine();
                changeCount++;
            }
        }

        if (changeCount == 0)
        {
            return ""; // No changes
        }

        // Prepend summary
        var header = $"-- {changeCount} record(s) modified\n\n";
        sb.Insert(sb.ToString().IndexOf('\n') + 1, header);

        return sb.ToString();
    }

    /// <summary>
    /// Format an int list to array format: [1,2,3]
    /// Empty list becomes [].
    /// </summary>
    private static string FormatIntArray(List<int> values)
    {
        if (values.Count == 0) return "[]";
        return $"[{string.Join(",", values)}]";
    }

    private static string EscapeSql(string value)
    {
        return value.Replace("'", "''");
    }

    private static bool ListsEqual(List<int> a, List<int> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
            if (a[i] != b[i]) return false;
        return true;
    }

    private static bool PrerequisitesEqual(List<JobPrerequisite> a, List<JobPrerequisite> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i].RequiredJobId != b[i].RequiredJobId) return false;
            if (a[i].RequiredLevel != b[i].RequiredLevel) return false;
            if (a[i].LevelRequirementPosition != b[i].LevelRequirementPosition) return false;
        }
        return true;
    }
}
