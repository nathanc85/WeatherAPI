namespace WeatherAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;

public class WeatherService : IWeatherService
{
    private readonly WeatherDbContext _db;

    public WeatherService(WeatherDbContext db)
    {
        _db = db;
    }

    public IEnumerable<WeatherForecast> GetWeatherForecasts(int days)
    {
        // Try to read existing forecasts from the database
        var existing = _db.WeatherForecasts.OrderBy(w => w.Date).Take(days).ToList();
        if (existing != null && existing.Count >= days)
            return existing;

        // Generate forecasts and persist them
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var generated = Enumerable.Range(1, days).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToArray();

        _db.WeatherForecasts.AddRange(generated);
        _db.SaveChanges();

        return generated;
    }
}
