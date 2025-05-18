using Marketplace.Web.Modules.Categories.Application.Interfaces;
using Marketplace.Web.Modules.Categories.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Categories.Application.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoryByIdHandler(ICategoryRepository repository)
            => _repository = repository;

        public async Task<Category?> Handle(GetCategoryByIdQuery query, CancellationToken token)
        {
            return await _repository.GetByIdAsync(query.CategoryId);
        }
    }
}
