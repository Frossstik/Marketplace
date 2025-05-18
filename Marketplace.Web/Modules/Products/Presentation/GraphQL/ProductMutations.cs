using Marketplace.Web.Modules.Products.Application.Commands.CreateProduct;
using MediatR;

namespace Marketplace.Web.Modules.Products.Presentation.GraphQL
{
    [ExtendObjectType("Mutation")]
    public class ProductMutations
    {
        public async Task<Guid> CreateProduct(
            [Service] IMediator mediator,
            string name,
            string description,
            decimal price,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(
                new CreateProductCommand(name, description, price),
                cancellationToken);
        }
    }
}
