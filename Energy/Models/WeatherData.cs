using System.Text.Json.Serialization;

namespace Energy.Service;

public class WeatherData
{
    [JsonPropertyName("hourly")]
    public Hourly Hourly { get; set; } = new();
}

public class Hourly
{
    [JsonPropertyName("time")]
    public List<DateTime> Time { get; set; } = new List<DateTime>();

    [JsonPropertyName("temperature_2m")]
    public List<double> Temperature { get; set; } = new List<double>();
}

public record WeatherData2(DateTime time, double temperature);