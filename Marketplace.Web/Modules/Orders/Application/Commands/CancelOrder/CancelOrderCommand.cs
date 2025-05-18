using MediatR;

namespace Marketplace.Web.Modules.Orders.Application.Commands.CancelOrder
{
    public record CancelOrderCommand(
    Guid OrderId,
    Guid UserId
) : IRequest;
}
