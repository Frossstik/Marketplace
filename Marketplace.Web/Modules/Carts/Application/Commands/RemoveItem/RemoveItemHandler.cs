using Marketplace.Web.Modules.Carts.Infrastructure;
using MediatR;

namespace Marketplace.Web.Modules.Carts.Application.Commands.RemoveItem
{
    public class RemoveItemHandler : IRequestHandler<RemoveItemCommand>
    {
        private readonly RedisCartRepository _repository;

        public RemoveItemHandler(RedisCartRepository repository)
            => _repository = repository;

        public async Task Handle(RemoveItemCommand command, CancellationToken token)
        {
            var cart = await _repository.GetCartAsync(command.UserId);
            if (cart == null) return;

            cart.RemoveItem(command.ProductId, command.Quantity);
            await _repository.SaveCartAsync(cart);
        }
    }
}
