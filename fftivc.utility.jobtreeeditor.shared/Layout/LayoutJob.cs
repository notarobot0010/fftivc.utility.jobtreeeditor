using System.Text.Json.Serialization;

namespace fftivc.utility.jobtreeeditor.shared.Layout;

public class LayoutJob
{
    [JsonPropertyName("key")]
    public int Key { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("position")]
    public LayoutPosition Position { get; set; } = new();

    [JsonPropertyName("comment")]
    public string Comment { get; set; } = "";

    [JsonPropertyName("requiredJobExp")]
    public List<int> RequiredJobExp { get; set; } = new();

    [JsonPropertyName("prerequisites")]
    public List<LayoutPrerequisite> Prerequisites { get; set; } = new();

    [JsonPropertyName("rightNeighbor")]
    public int RightNeighbor { get; set; }

    [JsonPropertyName("downNeighbor")]
    public int DownNeighbor { get; set; }

    [JsonPropertyName("leftNeighbor")]
    public int LeftNeighbor { get; set; }

    [JsonPropertyName("upNeighbor")]
    public int UpNeighbor { get; set; }
}
