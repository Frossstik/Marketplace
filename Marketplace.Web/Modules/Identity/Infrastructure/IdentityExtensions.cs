using Marketplace.Web.Modules.Identity.Domain.Entities;
using Marketplace.Web.Modules.Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Web.Modules.Identity.Infrastructure
{
    public static class IdentityExtensions
    {
        public static async Task AddToRoleEnumAsync(this UserManager<User> manager, User user, RoleEnum role)
            => await manager.AddToRoleAsync(user, role.ToString());

        public static async Task<bool> IsInRoleEnumAsync(this UserManager<User> manager, User user, RoleEnum role)
            => await manager.IsInRoleAsync(user, role.ToString());
    }
}
