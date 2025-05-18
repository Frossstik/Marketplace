using Marketplace.Web.Modules.Categories.Application.Commands.CreateCategory;

namespace Marketplace.Web.Modules.Categories.Presentation.GraphQL.Types
{
    public class CategoryInputType : InputObjectType<CreateCategoryCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateCategoryCommand> descriptor)
        {
            descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.CreatorSellerId).Type<IdType>();
        }
    }
}
