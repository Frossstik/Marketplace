using Marketplace.Web.Modules.Identity.Application.Commands.Register;
using Marketplace.Web.Modules.Identity.Application.DTO;
using Marketplace.Web.Modules.Identity.Domain.Enums;

namespace Marketplace.Web.Modules.Identity.Presentation.GraphQL.Types
{
    public class RegisterInputType : InputObjectType<RegisterCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterCommand> descriptor)
        {
            descriptor.Field(x => x.Email).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Password).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.FirstName).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.LastName).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.CompanyName).Type<StringType>();
            descriptor.Field(x => x.Role)
                .Type<RoleEnumType>()
                .DefaultValue(RoleEnum.Client)
                .Description("Роль пользователя (по умолчанию: Client)");
        }
    }
}
