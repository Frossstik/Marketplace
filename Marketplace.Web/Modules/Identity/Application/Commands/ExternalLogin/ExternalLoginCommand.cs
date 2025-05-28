using Marketplace.Web.Modules.Identity.Application.DTO;
using MediatR;

namespace Marketplace.Web.Modules.Identity.Application.Commands.ExternalLogin
{
    public record ExternalLoginCommand(
        string Email,
        string FirstName,
        string LastName,
        string? CompanyName,
        string Provider
    ) : IRequest<AuthResponse>;
}
