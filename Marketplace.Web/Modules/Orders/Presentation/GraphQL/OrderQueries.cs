using Marketplace.Web.Modules.Orders.Application.Queries.GetOrderById;
using Marketplace.Web.Modules.Orders.Application.Queries.GetUserOrders;
using Marketplace.Web.Modules.Orders.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Orders.Presentation.GraphQL
{
    [ExtendObjectType("Query")]
    public class OrderQueries
    {
        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<Order>> GetUserOrders(
            [Service] IMediator mediator,
            Guid userId,
            CancellationToken token)
        {
            return await mediator.Send(new GetUserOrdersQuery(userId), token);
        }

        public async Task<Order> GetOrderById(
            [Service] IMediator mediator,
            Guid orderId,
            Guid userId,
            CancellationToken token)
        {
            return await mediator.Send(new GetOrderByIdQuery(orderId, userId), token);
        }
    }

}
