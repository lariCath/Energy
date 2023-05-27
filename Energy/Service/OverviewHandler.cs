namespace Energy.Service;

public class OverviewHandler
{
    private readonly ApiService apiService;
    private readonly CalculationService calculationService;

    public OverviewHandler(ApiService apiService, CalculationService calculationService) 
    {
        this.apiService = apiService;
        this.calculationService = calculationService;
    }

    public async Task<List<EnergyData2>> GetBestTimeCar(List<DateTime[]> timeframes)
    {
        var trafficData = await apiService.GetTrafficLightData();

        return calculationService.GetCarData(trafficData, timeframes);
    }

    public async Task GetBestTimeDevice(List<DateTime> timeframes)
    {
        var trafficData = await apiService.GetTrafficLightData();

        return calculationService.GetDeviceData(trafficData, timeframes);
    }
}
