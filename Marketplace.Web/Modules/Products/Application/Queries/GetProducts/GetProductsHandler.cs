using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Modules.Products.Application.Queries.GetProducts
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
    {
        private readonly AppDbContext _dbContext;

        public GetProductsHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
