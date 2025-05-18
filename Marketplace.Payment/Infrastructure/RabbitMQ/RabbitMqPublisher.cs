using Marketplace.Payment.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace Marketplace.Payment.Infrastructure.RabbitMQ
{
    public class RabbitMqPublisher : IMessageBusPublisher
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private bool _disposed;

        private RabbitMqPublisher(IConnection connection, IChannel channel)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        }

        public static async Task<RabbitMqPublisher> CreateAsync(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("RabbitMQ connection string cannot be null or empty", nameof(connectionString));
            }

            try
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(connectionString)
                };

                var connection = await factory.CreateConnectionAsync();
                var channel = await connection.CreateChannelAsync();

                return new RabbitMqPublisher(connection, channel);
            }
            catch (UriFormatException ex)
            {
                throw new ArgumentException("Invalid RabbitMQ connection URI format", nameof(connectionString), ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to initialize RabbitMQ connection", ex);
            }
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
        {
            if (_disposed)
                throw new ObjectDisposedException("RabbitMqPublisher is disposed");

            var exchangeName = typeof(T).Name;

            await _channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await _channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: string.Empty,
                body: body,
                cancellationToken: cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;
            _disposed = true;

            if (_channel?.IsOpen == true)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }

            if (_connection?.IsOpen == true)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
