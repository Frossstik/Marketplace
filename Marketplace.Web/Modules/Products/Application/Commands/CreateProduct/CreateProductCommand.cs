using MediatR;

namespace Marketplace.Web.Modules.Products.Application.Commands.CreateProduct
{
    public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price
) : IRequest<Guid>;
}
