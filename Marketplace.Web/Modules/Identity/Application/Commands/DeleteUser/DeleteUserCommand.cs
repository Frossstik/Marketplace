using MediatR;

namespace Marketplace.Web.Modules.Identity.Application.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid UserId, Guid CurrentUserId) : IRequest<bool>;
}
