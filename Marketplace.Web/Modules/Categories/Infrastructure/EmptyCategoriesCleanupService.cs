using Marketplace.Web.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class EmptyCategoriesCleanupService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<EmptyCategoriesCleanupService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(6);

    public EmptyCategoriesCleanupService(IServiceProvider services, ILogger<EmptyCategoriesCleanupService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var emptyCategories = await dbContext.Categories
                    .Where(c => !dbContext.Products.Any(p => p.CategoryId == c.Id))
                    .ToListAsync(stoppingToken);

                foreach (var category in emptyCategories)
                {
                    dbContext.Categories.Remove(category);
                    _logger.LogInformation($"Deleted empty category: {category.Id} - {category.Name}");
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up empty categories");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}