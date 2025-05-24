using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Modules.Products.Application.Queries.GetProducts
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<Product>>
    {
        private readonly AppDbContext _context;

        public GetProductsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }
    }
}
