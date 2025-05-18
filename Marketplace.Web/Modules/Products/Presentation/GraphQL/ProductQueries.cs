using Marketplace.Web.Modules.Products.Application.Queries.GetProducts;
using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Products.Presentation.GraphQL
{
    [ExtendObjectType("Query")]
    public class ProductQueries
    {
        public async Task<IEnumerable<Product>> GetProducts(
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetProductsQuery(), cancellationToken);
        }
    }
}
