using Marketplace.Web.Modules.Orders.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Orders.Application.Queries.GetOrderById
{
    public record GetOrderByIdQuery(
    Guid OrderId,
    Guid UserId
) : IRequest<Order>;
}
