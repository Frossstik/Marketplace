using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Products.Application.Queries.GetProducts
{
    public record GetProductsQuery() : IRequest<List<Product>>;
}
