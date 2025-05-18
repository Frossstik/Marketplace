using Marketplace.Web.Modules.Carts.Domain.Entities;
using Marketplace.Web.Modules.Carts.Infrastructure;
using MediatR;

namespace Marketplace.Web.Modules.Carts.Application.Queries.GetCart
{
    public class GetCartHandler : IRequestHandler<GetCartQuery, Cart>
    {
        private readonly RedisCartRepository _repository;

        public GetCartHandler(RedisCartRepository repository)
            => _repository = repository;

        public async Task<Cart> Handle(GetCartQuery query, CancellationToken token)
        {
            return await _repository.GetCartAsync(query.UserId)
                   ?? new Cart(query.UserId);
        }
    }
}
