using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Products.Application.Commands.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly AppDbContext _context;

        public UpdateProductHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                throw new ArgumentException(nameof(product));
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Count = request.Count;
            product.ImagePaths = request.ImagePaths;
            product.CategoryId = request.CategoryId;

            if (product.Count <= 0)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return product;
        }
    }
}
