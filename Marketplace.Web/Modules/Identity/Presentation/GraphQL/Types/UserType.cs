using Marketplace.Web.Modules.Identity.Domain.Entities;

namespace Marketplace.Web.Modules.Identity.Presentation.GraphQL.Types
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Field(u => u.Id).Type<NonNullType<IdType>>();
            descriptor.Field(u => u.Email).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.FirstName).Type<StringType>();
            descriptor.Field(u => u.LastName).Type<StringType>();
            descriptor.Field(u => u.CompanyName).Type<StringType>();
            descriptor.Field(u => u.CreatedAt).Type<DateTimeType>();
        }
    }
}
