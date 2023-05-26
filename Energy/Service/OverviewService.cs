using Serilog;

namespace Energy.Service;

public class OverviewService
{
    private readonly IEnergyApi dataAPI;
    private readonly IWeatherApi weatherApi;

    public OverviewService(IEnergyApi dataAPI, IWeatherApi weatherApi)
    {
        this.dataAPI = dataAPI;
        this.weatherApi = weatherApi;
    }

    public async Task<List<EnergyData2>> GetDataAsync()
    {
        try
        {
            DateTime startTime = new(2023, 05, 25, 17, 0, 0);
            DateTime endTime = new(2023, 05, 25, 18, 0, 0);

            var start = startTime.ToString("yyyy-MM-dd HH:mm");
            var end = endTime.ToString("yyyy-MM-dd HH:mm");

            var queryparams = new QueryParamsEnergy("de", start, end);
            var rawData = await dataAPI.GetData(queryparams);

            var result = new List<EnergyData2>();
            var lists = rawData.Content;

            for (int i = 0; i < lists!.TimeStamp.Count; i++)
            {
                var d = new EnergyData2(lists.TimeStamp[i], lists.RenewableShare[i]);
                result.Add(d);
            }

            return result;
        }
        catch (Exception ex)
        {
            //TODO
            Log.Error(ex, "An errror occurred!");
            return new List<EnergyData2>();
        }
    }

    public async Task<List<WeatherData2>?> GetWeather()
    {
        try
        {
            DateTime time = new(2023, 05, 25, 17, 0, 0);
            double longitude = 12.10;
            double latitude = 49.01;

            var start = "temperature_2m";           //time.ToString("yyyy-MM-ddTHH:mm");

            var queryparams = new QueryParamsWeather(latitude, longitude, start, 1);
            var rawData = await weatherApi.GetData(queryparams);

            var result = new List<WeatherData2>();
            var lists = rawData.Content!.Hourly;

            for (int i = 0; i < lists!.Time.Count; i++)
            {
                var d = new WeatherData2(lists.Time[i], lists.Temperature[i]);
                result.Add(d);
            }

            return result;
        }
        catch (Exception ex)
        {
            //TODO
            Log.Error(ex, "An errror occurred!");
            return null;
        }
    }
}
