using Marketplace.Web.Modules.Categories.Application.Interfaces;
using Marketplace.Web.Modules.Categories.Domain.Entities;
using Marketplace.Web.Modules.Categories.Domain.Enums;

namespace Marketplace.Web.Modules.Categories.Infrastructure
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
            => _repository = repository;

        public async Task<Category> GetOrCreateAsync(string name, Guid? creatorSellerId)
        {
            var category = await _repository.GetByNameAsync(name);
            if (category != null) return category;

            category = new Category
            {
                Name = name,
                CreatorSellerId = creatorSellerId,
                Status = creatorSellerId.HasValue
                    ? CategoryStatusEnum.Pending
                    : CategoryStatusEnum.Active
            };

            await _repository.AddAsync(category);
            return category;
        }
    }

}
