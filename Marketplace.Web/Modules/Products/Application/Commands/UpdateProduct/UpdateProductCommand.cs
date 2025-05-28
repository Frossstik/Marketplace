using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Products.Application.Commands.UpdateProduct
{
    public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Count,
    List<string> ImagePaths,
    Guid CategoryId
) : IRequest<Product>;
}
