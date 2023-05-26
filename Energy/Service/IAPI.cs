using Refit;
using System.Text.Json.Serialization;

namespace Energy.Service;
public interface IAPI
{
    [Get("/total_power")]
    Task<ApiResponse<Energy>> GetEmployees();

    //[Get("/employee/{id}")]
    //Task<ApiResponse<EmployeeAPIRaw>> GetEmployeeById(int id);
}

public class Energy
{
    [JsonPropertyName("Renewable share of load (%)")]
    public List<double> RenewableShare { get; set; } = new List<double>();
}
