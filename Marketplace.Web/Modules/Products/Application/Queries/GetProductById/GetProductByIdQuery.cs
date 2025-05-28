using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Products.Application.Queries.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IRequest<Product?>;
}
