using MediatR;

namespace Marketplace.Web.Modules.Carts.Application.Commands.AddItem
{
    public record AddItemCommand(
    Guid UserId,
    Guid ProductId,
    string ProductName,
    decimal Price,
    int Quantity = 1
) : IRequest;
}
