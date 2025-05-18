using Marketplace.Web.Modules.Carts.Application.Commands.AddItem;
using MediatR;

namespace Marketplace.Web.Modules.Carts.Presentation.GraphQL
{
    [ExtendObjectType("Mutation")]
    public class CartMutations
    {
        public async Task<bool> AddCartItem(
            [Service] IMediator mediator,
            Guid userId,
            Guid productId,
            string productName,
            decimal price,
            int quantity = 1,
            CancellationToken token = default)
        {
            await mediator.Send(
                new AddItemCommand(userId, productId, productName, price, quantity),
                token);
            return true;
        }
    }
}
