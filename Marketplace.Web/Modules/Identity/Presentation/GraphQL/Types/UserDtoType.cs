using Marketplace.Web.Modules.Identity.Application.Queries.GetUsers;

namespace Marketplace.Web.Modules.Identity.Presentation.GraphQL.Types
{
    public class UserDtoType : ObjectType<UserDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserDto> descriptor)
        {
            descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
            descriptor.Field(x => x.Email).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.FirstName).Type<StringType>();
            descriptor.Field(x => x.LastName).Type<StringType>();
            descriptor.Field(x => x.CompanyName).Type<StringType>();
            descriptor.Field(x => x.CreatedAt).Type<NonNullType<DateTimeType>>();
            descriptor.Field(x => x.Roles).Type<ListType<StringType>>();
        }
    }
}
