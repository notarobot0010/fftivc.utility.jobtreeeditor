using System.Text.Json.Serialization;

namespace fftivc.utility.jobtreeeditor.shared.Layout;

public class LayoutPosition
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }
}
