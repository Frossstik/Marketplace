using MediatR;

namespace Marketplace.Web.Modules.Products.Application.Commands.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : IRequest<bool>;
}
