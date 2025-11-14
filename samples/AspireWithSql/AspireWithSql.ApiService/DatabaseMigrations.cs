using AspireWithSql.ApiService.Data;
using Microsoft.EntityFrameworkCore;

namespace AspireWithSql.ApiService
{
    public class DatabaseMigrations(IServiceProvider serviceProvider, ILogger<DatabaseMigrations> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<WeatherForecastContext>();

                await MigrateDatabaseAsync(dbContext, cancellationToken);
                await SeedDatabaseAsync(dbContext, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                logger.LogWarning(ex, "Database migration cancelled.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while applying database migrations.");
            }
        }

        private static async Task MigrateDatabaseAsync(WeatherForecastContext dbContext, CancellationToken cancellationToken)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                await dbContext.Database.MigrateAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            });
        }

        private static async Task SeedDatabaseAsync(WeatherForecastContext dbContext, CancellationToken cancellationToken)
        {
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            var rnd = new Random();
            var items = Enumerable.Range(1, 10).Select(i => new WeatherForecastEntity
            {
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(i)),
                TemperatureC = rnd.Next(-20, 55),
                Summary = summaries[rnd.Next(summaries.Length)]
            });

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

                foreach (var item in items)
                {
                    if (dbContext.Forecasts.Where(d => d.Date == item.Date).SingleOrDefault() == null)
                        await dbContext.Forecasts.AddAsync(item, cancellationToken);
                }

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            });
        }
    }
}
