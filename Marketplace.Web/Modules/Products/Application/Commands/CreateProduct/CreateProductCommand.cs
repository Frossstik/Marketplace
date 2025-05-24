using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Products.Application.Commands.CreateProduct
{
    public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Count,
    List<string> ImagePaths,
    Guid CategoryId
) : IRequest<Product>;
}
