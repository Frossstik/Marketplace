using Microsoft.AspNetCore.Identity;

namespace Marketplace.Web.Modules.Identity.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? CompanyName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
