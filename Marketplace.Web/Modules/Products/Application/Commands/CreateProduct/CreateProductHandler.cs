using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Products.Domain;
using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Products.Application.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly AppDbContext _context;

        public CreateProductHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Count = request.Count,
                ImagePath = request.ImagePath,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product;
        }
    }
}
