using Refit;

namespace Energy.Service;

public interface IWeatherApi
{
    [Get("/forecast")]
    Task<ApiResponse<WeatherData>> GetData(QueryParamsWeather param);

}
