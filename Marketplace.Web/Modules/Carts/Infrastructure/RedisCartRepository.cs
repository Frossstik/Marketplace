using Marketplace.Web.Modules.Carts.Domain.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace Marketplace.Web.Modules.Carts.Infrastructure
{
    public class RedisCartRepository
    {
        private readonly IDatabase _db;
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public RedisCartRepository(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<Cart?> GetCartAsync(Guid userId)
        {
            var value = await _db.StringGetAsync($"cart:{userId}");
            return value.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(value!, _options);
        }

        public async Task SaveCartAsync(Cart cart)
        {
            await _db.StringSetAsync(
                $"cart:{cart.UserId}",
                JsonSerializer.Serialize(cart, _options),
                TimeSpan.FromDays(7) // TTL 7 дней
            );
        }

        public async Task DeleteCartAsync(Guid userId)
        {
            await _db.KeyDeleteAsync($"cart:{userId}");
        }
    }
}
