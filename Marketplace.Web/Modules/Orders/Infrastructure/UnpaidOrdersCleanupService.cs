using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Orders.Domain.Enums;

namespace Marketplace.Web.Modules.Orders.Infrastructure
{
    public class UnpaidOrdersCleanupService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<UnpaidOrdersCleanupService> _logger;
        private static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(1);

        public UnpaidOrdersCleanupService(IServiceProvider services, ILogger<UnpaidOrdersCleanupService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(CleanupInterval, stoppingToken);

                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var threshold = DateTime.UtcNow.AddHours(-1);

                    var expiredOrders = db.Orders
                        .Where(o => o.Status == OrderStatus.PendingPayment && o.CreatedAt < threshold);

                    db.Orders.RemoveRange(expiredOrders);
                    var count = await db.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation("Удалено {Count} просроченных неоплаченных заказов", count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при удалении просроченных заказов");
                }
            }
        }
    }
}
