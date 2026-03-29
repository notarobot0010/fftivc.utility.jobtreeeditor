using System.Text.Json.Serialization;

namespace fftivc.utility.jobtreeeditor.shared.Layout;

public class LayoutJob
{
    [JsonPropertyName("key")]
    public int Key { get; set; }

    [JsonPropertyName("name")]
    public string DisplayName { get; set; } = "";

    [JsonPropertyName("uibSlotBinding")]
    public string UibSlotBinding { get; set; } = "";

    [JsonPropertyName("position")]
    public LayoutPosition Position { get; set; } = new();

    [JsonPropertyName("comment")]
    public string Comment { get; set; } = "";

    [JsonPropertyName("requiredJobExp")]
    public List<int> RequiredJobExp { get; set; } = [];

    [JsonPropertyName("prerequisites")]
    public List<LayoutPrerequisite> Prerequisites { get; set; } = [];

    [JsonPropertyName("neighbors")]
    public LayoutNeighbors Neighbors { get; set; } = new();
}
