using Marketplace.Web.Modules.Categories.Domain.Entities;
using Marketplace.Web.Modules.Categories.Domain.Enums;

namespace Marketplace.Web.Modules.Categories.Presentation.GraphQL.Types
{
    public class CategoryType : ObjectType<Category>
    {
        protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Status).Type<NonNullType<EnumType<CategoryStatusEnum>>>();
            descriptor.Field(c => c.CreatedAt).Type<NonNullType<DateTimeType>>();
        }
    }
}
