using Marketplace.Web.Modules.Identity.Domain.Enums;

namespace Marketplace.Web.Modules.Identity.Presentation.GraphQL.Types
{
    public class RoleEnumType : EnumType<RoleEnum>
    {
        protected override void Configure(IEnumTypeDescriptor<RoleEnum> descriptor)
        {
            descriptor.Name("Role");
        }
    }
}
