using System.Text.Json.Serialization;

namespace Energy.Service;

public class EnergyData3
{
    [JsonPropertyName("xAxisValues")]
    public List<long> Unixstamps { get; set; } = new List<long>();

    [JsonPropertyName("data")]
    public List<double?> Data { get; set; } = new List<double?>();
}