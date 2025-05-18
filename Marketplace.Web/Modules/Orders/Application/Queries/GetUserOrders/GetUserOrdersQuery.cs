using Marketplace.Web.Modules.Orders.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Orders.Application.Queries.GetUserOrders
{
    public record GetUserOrdersQuery(Guid UserId) : IRequest<IEnumerable<Order>>;
}
