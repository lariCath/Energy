namespace Energy.Service;

public record QueryParamsWeather(double latitude, double longitude, string hourly, int forecast_days);