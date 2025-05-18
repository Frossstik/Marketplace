using Marketplace.Web.Modules.Orders.Application.Commands.CancelOrder;
using Marketplace.Web.Modules.Orders.Application.Commands.CreateOrder;
using Marketplace.Web.Modules.Orders.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Orders.Presentation.GraphQL
{
    [ExtendObjectType("Mutation")]
    public class OrderMutations
    {
        public async Task<Guid> CreateOrder(
            [Service] IMediator mediator,
            Guid userId,
            List<OrderItemInput> items,
            CancellationToken token)
        {
            return await mediator.Send(
                new CreateOrderCommand(
                    userId,
                    items.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        UnitPrice = i.UnitPrice,
                        Quantity = i.Quantity
                    }).ToList()),
                token);
        }

        public async Task<bool> CancelOrder(
            [Service] IMediator mediator,
            Guid orderId,
            Guid userId,
            CancellationToken token)
        {
            await mediator.Send(new CancelOrderCommand(orderId, userId), token);
            return true;
        }
    }

    public record OrderItemInput(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice,
        int Quantity);
}
