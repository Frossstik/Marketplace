using Marketplace.Web.Modules.Identity.Domain.Entities;
using Marketplace.Web.Modules.Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Marketplace.Web.Modules.Identity.Infrastructure
{
    public class RoleConverter : ValueConverter<RoleEnum, string>
    {
        public RoleConverter() : base(
            v => v.ToString(),
            v => (RoleEnum)Enum.Parse(typeof(RoleEnum), v))
        { }
    }


}
