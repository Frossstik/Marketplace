using Marketplace.Web.Modules.Categories.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Categories.Application.Queries.GetCategories
{
    public record GetCategoriesQuery(
) : IRequest<IEnumerable<Category>>;
}
