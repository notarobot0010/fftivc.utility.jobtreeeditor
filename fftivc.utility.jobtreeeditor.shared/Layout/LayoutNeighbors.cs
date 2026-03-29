using System.Text.Json.Serialization;

namespace fftivc.utility.jobtreeeditor.shared.Layout;

public class LayoutNeighbors
{
    [JsonPropertyName("rightNeighbor")]
    public int RightNeighbor { get; set; }

    [JsonPropertyName("rightFallback")]
    public int RightFallback { get; set; }

    [JsonPropertyName("downRightNeighbor")]
    public int DownRightNeighbor { get; set; }

    [JsonPropertyName("downRightFallback")]
    public int DownRightFallback { get; set; }

    [JsonPropertyName("downNeighbor")]
    public int DownNeighbor { get; set; }

    [JsonPropertyName("downFallback")]
    public int DownFallback { get; set; }

    [JsonPropertyName("downLeftNeighbor")]
    public int DownLeftNeighbor { get; set; }

    [JsonPropertyName("downLeftFallback")]
    public int DownLeftFallback { get; set; }

    [JsonPropertyName("leftNeighbor")]
    public int LeftNeighbor { get; set; }

    [JsonPropertyName("leftFallback")]
    public int LeftFallback { get; set; }

    [JsonPropertyName("upLeftNeighbor")]
    public int UpLeftNeighbor { get; set; }

    [JsonPropertyName("upLeftFallback")]
    public int UpLeftFallback { get; set; }

    [JsonPropertyName("upNeighbor")]
    public int UpNeighbor { get; set; }

    [JsonPropertyName("upFallback")]
    public int UpFallback { get; set; }

    [JsonPropertyName("upRightNeighbor")]
    public int UpRightNeighbor { get; set; }

    [JsonPropertyName("upRightFallback")]
    public int UpRightFallback { get; set; }
}
