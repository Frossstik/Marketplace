using Marketplace.Web.Modules.Categories.Application.Interfaces;
using Marketplace.Web.Modules.Categories.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Categories.Application.Commands.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _repository;

        public CreateCategoryHandler(ICategoryRepository repository)
            => _repository = repository;

        public async Task<Guid> Handle(CreateCategoryCommand command, CancellationToken token)
        {
            var existingCategory = await _repository.GetByNameAsync(command.Name);

            if (existingCategory != null)
            {
                return existingCategory.Id;
            }

            var category = new Category
            {
                Name = command.Name,
                CreatorSellerId = command.CreatorSellerId,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(category);
            return category.Id;
        }
    }
}