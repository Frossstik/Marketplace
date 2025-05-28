using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Products.Application.Queries.GetProductById;
using Marketplace.Web.Modules.Products.Application.Queries.GetProducts;
using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Modules.Products.Presentation.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class ProductQueries
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Product> GetProducts(
        [Service] AppDbContext context)
        {
            return context.Products
                .Include(p => p.Category)
                .AsQueryable();
        }

        public async Task<Product?> GetProductById(
            [ID] Guid id,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        }
    }
}