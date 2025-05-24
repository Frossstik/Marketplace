namespace Marketplace.Web.Infrastructure.RabbitMQ
{
    public interface IMessageBusPublisher : IAsyncDisposable
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class;
    }
}
