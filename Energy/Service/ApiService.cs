using Energy.Models;
using Serilog;

namespace Energy.Service;

public class ApiService
{
    private readonly IEnergyApi dataAPI;
    private readonly IWeatherApi weatherApi;

    public ApiService(IEnergyApi dataAPI, IWeatherApi weatherApi)
    {
        this.dataAPI = dataAPI;
        this.weatherApi = weatherApi;
    }

    private DateTime UnixToDateTime(long unixTime)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        return dateTimeOffset.DateTime;
    }

    public async Task<List<EnergyData2>> GetDataAsync()
    {
        try
        {
            DateTime startTime = new(2023, 05, 25, 17, 0, 0);
            DateTime endTime = new(2023, 05, 25, 18, 0, 0);

            var start = startTime.ToString("yyyy-MM-dd HH:mm");
            var end = endTime.ToString("yyyy-MM-dd HH:mm");

            var queryparams = new QueryParamsTotalPower("de", start, end);
            var rawData = await dataAPI.GetData(queryparams);

            var result = new List<EnergyData2>();
            var lists = rawData.Content;

            for (int i = 0; i < lists!.TimeStamp.Count; i++)
            {
                var time = UnixToDateTime(lists.TimeStamp[i]);
                var d = new EnergyData2(time, lists.RenewableShare[i]);
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

    public async Task<ShareData?> GetWindAndSolarData()
    {
        try
        {
            var solar = await dataAPI.GetSolarData("de");
            var windOn = await dataAPI.GetWindOnshoreData("de");
            var windOff = await dataAPI.GetWindOffshoreData("de");

            ShareData result = new()
            {
                SolarShare = GetEnergyData2(solar.Content),
                WindOffshoreShare = GetEnergyData2(windOff.Content),
                WindOnshoreShare = GetEnergyData2(windOn.Content)
            };

            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Something went wrong:");
            return null;
        }
    }

    private List<EnergyData2> GetEnergyData2(List<EnergyData3> rawdata)
    {
        var res = new List<EnergyData2>();

        for(int i = 0; i< rawdata[0].Unixstamps.Count; i++)
        {
            var time = UnixToDateTime(rawdata[0].Unixstamps[i]);

            var a = new EnergyData2(time, rawdata[1].Data[i]);
            res.Add(a);
        }

        return res;
    }

    public async Task<List<EnergyData2>?> GetTrafficLightData()
    {
        try
        {
            var rawdata = (await dataAPI.GetTrafficSignal("de")).Content!;

            var res = new List<EnergyData2>();

            for (int i = 0; i < rawdata[0].Unixstamps.Count; i++)
            {
                var time = UnixToDateTime(rawdata[0].Unixstamps[i]);

                var a = new EnergyData2(time, rawdata[0].Data[i]);
                res.Add(a);
            }

            return res;
        }
        catch(Exception ex)
        {
            Log.Error(ex, "Something went wrong!");
            return null;
        }
    }

    public async Task<List<WeatherData2>?> GetWeather()
    {
        try
        {
            double longitude = 12.10;
            double latitude = 49.01;

            var start = "temperature_2m";

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
