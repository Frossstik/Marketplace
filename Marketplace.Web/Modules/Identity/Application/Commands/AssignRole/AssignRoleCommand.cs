using Marketplace.Web.Modules.Identity.Domain.Enums;
using MediatR;

namespace Marketplace.Web.Modules.Identity.Application.Commands.AssignRole
{
    public record AssignRoleCommand(
        Guid UserId,
        RoleEnum Role
    ) : IRequest;
}
