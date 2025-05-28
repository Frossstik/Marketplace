using Marketplace.Web.Modules.Categories.Application.Queries.GetCategories;
using Marketplace.Web.Modules.Categories.Application.Queries.GetCategoryById;
using Marketplace.Web.Modules.Categories.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Categories.Presentation.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class CategoryQueries
    {
        public async Task<IEnumerable<Category>> GetCategories(
        [Service] IMediator mediator,
        CancellationToken token = default)
        {
            return await mediator.Send(new GetCategoriesQuery(), token);
        }

        public async Task<Category?> GetCategoryById(
        [Service] IMediator mediator,
        Guid categoryId,
        CancellationToken token)
        {
            return await mediator.Send(new GetCategoryByIdQuery(categoryId), token);
        }
    }
}
