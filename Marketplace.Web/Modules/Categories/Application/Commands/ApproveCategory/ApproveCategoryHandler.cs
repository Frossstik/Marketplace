using Marketplace.Web.Modules.Categories.Application.Interfaces;
using Marketplace.Web.Modules.Categories.Domain.Enums;
using MediatR;

namespace Marketplace.Web.Modules.Categories.Application.Commands.ApproveCategory
{
    public class ApproveCategoryHandler : IRequestHandler<ApproveCategoryCommand>
    {
        private readonly ICategoryRepository _repository;

        public ApproveCategoryHandler(ICategoryRepository repository)
            => _repository = repository;

        public async Task Handle(ApproveCategoryCommand command, CancellationToken token)
        {
            var category = await _repository.GetByIdAsync(command.CategoryId);
            if (category == null) throw new ArgumentException("Category not found");

            category.Status = CategoryStatusEnum.Active;
            await _repository.UpdateAsync(category);
        }
    }

}
