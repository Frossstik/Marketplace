using MediatR;

namespace Marketplace.Web.Modules.Carts.Application.Commands.ClearCart
{

    public record ClearCartCommand(
        Guid UserId
    ) : IRequest;

}
