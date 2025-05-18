using Marketplace.Web.Modules.Products.Domain.Entities;

namespace Marketplace.Web.Modules.Products.Presentation.GraphQL
{
    public class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Field(p => p.Id).Type<IdType>();
            descriptor.Field(p => p.Name).Type<StringType>();
            descriptor.Field(p => p.Description).Type<StringType>();
            descriptor.Field(p => p.Price).Type<DecimalType>();
            descriptor.Field(p => p.CreatedAt).Type<DateTimeType>();
        }
    }
}
