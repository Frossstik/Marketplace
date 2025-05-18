using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Categories.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Modules.Categories.Application.Commands.DeleteEmptyCategory
{
    public class DeleteEmptyCategoryHandler : IRequestHandler<DeleteEmptyCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly AppDbContext _dbContext;

        public DeleteEmptyCategoryHandler(
            ICategoryRepository categoryRepository,
            AppDbContext dbContext)
        {
            _categoryRepository = categoryRepository;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteEmptyCategoryCommand command, CancellationToken token)
        {
            var category = await _categoryRepository.GetByIdAsync(command.CategoryId);
            if (category == null) return false;

            // Проверяем, есть ли товары в категории
            bool hasProducts = await _dbContext.Products
                .AnyAsync(p => p.CategoryId == command.CategoryId, token);

            if (hasProducts) return false;

            await _categoryRepository.DeleteAsync(category);
            return true;
        }
    }
}
