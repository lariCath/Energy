using System.Text.Json.Serialization;

namespace Energy.Service;

public class EnergyData
{
    [JsonPropertyName("Renewable share of load (%)")]
    public List<double> RenewableShare { get; set; } = new List<double>();

    [JsonPropertyName("xAxisValues (Unix timestamp)")]
    public List<long> TimeStamp { get; set; } = new List<long>();
}
