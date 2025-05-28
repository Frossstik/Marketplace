using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Orders.Domain.Events;
using Marketplace.Web.Modules.Orders.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Marketplace.Web.Modules.Orders.Domain.Events.Marketplace.Web.Modules.Orders.Domain.Events;

namespace Marketplace.Web.Infrastructure.RabbitMQ.Consumers
{
    public class PaymentCompletedConsumer : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;

        public PaymentCompletedConsumer(IServiceProvider services, IConfiguration config)
        {
            _services = services;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_config.GetConnectionString("RabbitMQ")!)
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            const string exchangeName = "PaymentCompletedEvent";

            await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout, durable: true, autoDelete: false, cancellationToken: stoppingToken);

            var queue = await channel.QueueDeclareAsync(queue: "", durable: false, exclusive: true, autoDelete: true, cancellationToken: stoppingToken);

            await channel.QueueBindAsync(queue.QueueName, exchangeName, routingKey: "", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = JsonSerializer.Deserialize<PaymentCompletedEvent>(body);

                    if (message is null) return;

                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var order = await db.Orders.FindAsync(new object[] { message.OrderId }, stoppingToken);
                    if (order == null) return;

                    order.MarkAsPaid();
                    await db.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    // TODO: потом добавить логи
                }
            };

            await channel.BasicConsumeAsync(
                queue: queue.QueueName,
                autoAck: true,
                consumer: consumer,
                cancellationToken: stoppingToken
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
