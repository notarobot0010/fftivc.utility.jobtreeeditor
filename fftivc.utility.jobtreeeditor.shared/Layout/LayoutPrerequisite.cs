using fftivc.utility.jobtreeeditor.shared.Enums;
using System.Text.Json.Serialization;

namespace fftivc.utility.jobtreeeditor.shared.Layout;

public class LayoutPrerequisite
{
    [JsonPropertyName("jobKey")]
    public int JobKey { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("requirementPosition")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LevelRequirementPosition RequirementPosition { get; set; }
}
