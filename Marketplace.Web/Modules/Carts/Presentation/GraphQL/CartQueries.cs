using Marketplace.Web.Modules.Carts.Application.Queries.GetCart;
using Marketplace.Web.Modules.Carts.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Carts.Presentation.GraphQL
{
    [ExtendObjectType("Query")]
    public class CartQueries
    {
        public async Task<Cart> GetCart(
            [Service] IMediator mediator,
            Guid userId,
            CancellationToken token)
        {
            return await mediator.Send(new GetCartQuery(userId), token);
        }
    }
}
