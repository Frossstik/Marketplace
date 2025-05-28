using Marketplace.Web.Modules.Products.Application.Commands.CreateProduct;

namespace Marketplace.Web.Modules.Products.Presentation.GraphQL.Types
{
    public class ProductInputType : InputObjectType<CreateProductCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateProductCommand> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Description).Type<StringType>();
            descriptor.Field(c => c.Price).Type<NonNullType<DecimalType>>();
            descriptor.Field(c => c.Count).Type<NonNullType<IntType>>();
            descriptor.Field(c => c.ImagePaths).Type<ListType<StringType>>();
            descriptor.Field(c => c.CategoryId).Type<NonNullType<IdType>>();
        }
    }
}
