using Marketplace.Payment.Infrastructure.Interfaces;

namespace Marketplace.Payment.Infrastructure.RabbitMQ
{
    public class RabbitMqDisposerService : IHostedService
    {
        private readonly IMessageBusPublisher _publisher;

        public RabbitMqDisposerService(IMessageBusPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
            => await Task.CompletedTask;

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_publisher is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
        }
    }
}
