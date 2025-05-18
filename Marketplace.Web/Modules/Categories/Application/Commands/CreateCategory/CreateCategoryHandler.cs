using Marketplace.Web.Modules.Categories.Application.Interfaces;
using Marketplace.Web.Modules.Categories.Domain.Entities;
using Marketplace.Web.Modules.Categories.Domain.Enums;
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
            var category = new Category
            {
                Name = command.Name,
                CreatorSellerId = command.CreatorSellerId,
                Status = command.CreatorSellerId.HasValue
                    ? CategoryStatusEnum.Pending
                    : CategoryStatusEnum.Active
            };

            await _repository.AddAsync(category);
            return category.Id;
        }
    }
}
