using Marketplace.Web.Modules.Identity.Application.DTO;
using Marketplace.Web.Modules.Identity.Domain.Enums;
using MediatR;

namespace Marketplace.Web.Modules.Identity.Application.Commands.Register
{
    public record RegisterCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string? CompanyName = null,
        RoleEnum Role = RoleEnum.Client
    ) : IRequest<AuthResponse>;
}
