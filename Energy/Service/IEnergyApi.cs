using Refit;

namespace Energy.Service;
public interface IEnergyApi
{
    [Get("/total_power")]
    Task<ApiResponse<EnergyData>> GetData(QueryParamsTotalPower param);

    [Get("/traffic_signal")]
    Task<ApiResponse<EnergyData3>> GetTrafficSignal(string country);

    [Get("/solar_share")]
    Task<ApiResponse<List<EnergyData3>>> GetSolarData(string country);

    [Get("/wind_onshore_share")]
    Task<ApiResponse<List<EnergyData3>>> GetWindOnshoreData(string country);

    [Get("/wind_offshore_share")]
    Task<ApiResponse<List<EnergyData3>>> GetWindOffshoreData(string country);
}

