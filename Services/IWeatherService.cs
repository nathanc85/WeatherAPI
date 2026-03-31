namespace WeatherAPI.Services;
public interface IWeatherService
{
    IEnumerable<WeatherForecast> GetWeatherForecasts(int days);
}
