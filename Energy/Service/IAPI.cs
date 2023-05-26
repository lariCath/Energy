using Refit;
using System.Text.Json.Serialization;

namespace Energy.Service;
public interface IAPI
{
    [Get("/total_power")]
    Task<ApiResponse<EnergyData>> GetData(QueryParams param);

    //[Get("/employee/{id}")]
    //Task<ApiResponse<EmployeeAPIRaw>> GetEmployeeById(int id);
}

public class EnergyData
{
    [JsonPropertyName("Renewable share of load (%)")]
    public List<double> RenewableShare { get; set; } = new List<double>();

    [JsonPropertyName("xAxisValues (Unix timestamp)")]
    public List<long> TimeStamp { get; set; } = new List<long>();
}

public record EnergyData2(long timeStamp, double renewableShare);


public record QueryParams(string country, string start, string end);