using Marketplace.Web.Modules.Orders.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Orders.Application.Commands.CreateOrder
{
    public record CreateOrderCommand(
    Guid UserId,
    List<OrderItem> Items
) : IRequest<Guid>;
}
