using Marketplace.Web.Modules.Identity.Application.DTO;

namespace Marketplace.Web.Modules.Identity.Presentation.GraphQL.Types
{
    public class AuthResponseType : ObjectType<AuthResponse>
    {
        protected override void Configure(IObjectTypeDescriptor<AuthResponse> descriptor)
        {
            descriptor.Field(x => x.Token).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.UserId).Type<NonNullType<IdType>>();
            descriptor.Field(x => x.Email).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Role).Type<RoleEnumType>();
        }
    }
}
