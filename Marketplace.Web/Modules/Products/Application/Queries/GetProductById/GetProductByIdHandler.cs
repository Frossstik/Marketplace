using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Modules.Products.Application.Queries.GetProductById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly AppDbContext _context;

        public GetProductByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        }
    }
}
