using System.Text.Json.Serialization;

namespace fftivc.utility.jobtreeeditor.uib;

public class LayoutEntry
{
    [JsonPropertyName("job")]
    public string Job { get; set; } = "";

    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }
}
