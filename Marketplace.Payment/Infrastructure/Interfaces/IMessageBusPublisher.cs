namespace Marketplace.Payment.Infrastructure.Interfaces
{
    public interface IMessageBusPublisher : IAsyncDisposable
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class;
    }
}
