using Microsoft.EntityFrameworkCore;

namespace AspireWithSql.ApiService.Data
{
    public class WeatherForecastContext(DbContextOptions<WeatherForecastContext> options) : DbContext(options)
    {
        public DbSet<WeatherForecastEntity> Forecasts => Set<WeatherForecastEntity>();
    }

    public class WeatherForecastEntity
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}
