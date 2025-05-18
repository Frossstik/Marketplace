using Marketplace.Web.Modules.Identity.Domain.Entities;

namespace Marketplace.Web.Modules.Identity.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
    }
}
