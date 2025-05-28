using HotChocolate.Types;
using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Categories.Domain.Entities;
using Marketplace.Web.Modules.Identity.Domain.Entities;
using Marketplace.Web.Modules.Identity.Presentation.GraphQL.Types;
using Marketplace.Web.Modules.Products.Domain.Entities;

namespace Marketplace.Web.Modules.Products.Presentation.GraphQL.Types
{
    public class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Field(p => p.Id).Type<NonNullType<IdType>>();
            descriptor.Field(p => p.Name).Type<StringType>();
            descriptor.Field(p => p.Description).Type<StringType>();
            descriptor.Field(p => p.Price).Type<DecimalType>();
            descriptor.Field(p => p.Count).Type<IntType>();
            descriptor.Field(p => p.ImagePaths).Type<ListType<StringType>>();
            descriptor.Field(p => p.CreatedAt).Type<DateTimeType>();
            descriptor.Field(p => p.Category)
                .Resolve(async context =>
                {
                    var product = context.Parent<Product>();
                    var dbContext = context.Service<AppDbContext>();
                    return await dbContext.Categories.FindAsync(product.CategoryId);
                })
                .Name("category");
            descriptor.Field("creator")
                .Type<NonNullType<UserType>>()
                .Resolve(context =>
                {
                    var product = context.Parent<Product>();
                    return new User
                    {
                        Id = product.CreatorId,
                        FirstName = product.CreatorName?.Split(' ')[0],
                        LastName = product.CreatorName?.Split(' ').Length > 1 ? product.CreatorName.Split(' ')[1] : null,
                        CompanyName = product.CreatorsCompanyName
                    };
                });
        }
    }
}
