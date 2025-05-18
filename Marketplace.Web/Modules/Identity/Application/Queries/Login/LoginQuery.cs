using Marketplace.Web.Modules.Identity.Application.DTO;
using MediatR;

namespace Marketplace.Web.Modules.Identity.Application.Queries.Login
{
    public record LoginQuery(
    string Email,
    string Password
) : IRequest<AuthResponse>;
}
