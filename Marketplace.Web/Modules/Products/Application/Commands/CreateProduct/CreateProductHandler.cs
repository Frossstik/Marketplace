using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Identity.Domain.Entities;
using Marketplace.Web.Modules.Products.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Marketplace.Web.Modules.Products.Application.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateProductHandler(
            AppDbContext context,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"Authenticated user not found in database");
            }

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                throw new ArgumentException($"Category with ID {request.CategoryId} not found");
            }

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Count = request.Count,
                ImagePaths = request.ImagePaths,
                CategoryId = request.CategoryId,
                CreatorId = Guid.Parse(userId),
                CreatorName = $"{user.FirstName} {user.LastName}".Trim(),
                CreatorsCompanyName = user.CompanyName?.Trim() ?? string.Empty
            };

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return product;
        }
    }
}