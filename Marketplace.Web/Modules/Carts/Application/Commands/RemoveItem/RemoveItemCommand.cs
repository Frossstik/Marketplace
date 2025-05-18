using MediatR;

namespace Marketplace.Web.Modules.Carts.Application.Commands.RemoveItem
{
    public record RemoveItemCommand(
    Guid UserId,
    Guid ProductId,
    int Quantity = 1
) : IRequest;
}
