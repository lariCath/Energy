using Refit;

namespace Energy.Service;
public interface IEnergyApi
{
    [Get("/total_power")]
    Task<ApiResponse<EnergyData>> GetData(QueryParams param);

}
