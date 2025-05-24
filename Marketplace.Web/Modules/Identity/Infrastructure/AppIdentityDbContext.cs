using Marketplace.Web.Modules.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Modules.Identity.Infrastructure
{
    public class AppIdentityDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }
    }


}
