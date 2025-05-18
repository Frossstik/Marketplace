using Marketplace.Web.Modules.Identity.Domain.Enums;

namespace Marketplace.Web.Modules.Identity.Application.DTO
{
    public record AuthResponse(
    string Token,
    Guid UserId,
    string Email,
    RoleEnum Role
);
}
