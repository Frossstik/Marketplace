using Marketplace.Web.Modules.Carts.Domain.Entities;
using Marketplace.Web.Modules.Carts.Infrastructure;
using MediatR;

namespace Marketplace.Web.Modules.Carts.Application.Commands.AddItem
{
    public class AddItemHandler : IRequestHandler<AddItemCommand>
    {
        private readonly RedisCartRepository _repository;

        public AddItemHandler(RedisCartRepository repository)
            => _repository = repository;

        public async Task Handle(AddItemCommand command, CancellationToken token)
        {
            var cart = await _repository.GetCartAsync(command.UserId)
                     ?? new Cart(command.UserId);

            cart.AddItem(new CartItem(
                command.ProductId,
                command.ProductName,
                command.Price,
                command.Quantity));

            await _repository.SaveCartAsync(cart);
        }
    }
}
