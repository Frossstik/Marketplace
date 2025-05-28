using Marketplace.Web.Infrastructure;
using MediatR;

namespace Marketplace.Web.Modules.Products.Application.Commands.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteProductHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
