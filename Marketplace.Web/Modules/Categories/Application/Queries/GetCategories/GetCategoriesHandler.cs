using Marketplace.Web.Modules.Categories.Application.Interfaces;
using Marketplace.Web.Modules.Categories.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Categories.Application.Queries.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoriesHandler(ICategoryRepository repository)
            => _repository = repository;

        public async Task<IEnumerable<Category>> Handle(
            GetCategoriesQuery query,
            CancellationToken token)
        {
            return await _repository.GetAllAsync();
        }
    }
}
