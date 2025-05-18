using Marketplace.Web.Modules.Categories.Domain.Entities;
using Marketplace.Web.Modules.Categories.Domain.Enums;
using MediatR;

namespace Marketplace.Web.Modules.Categories.Application.Queries.GetCategories
{
    public record GetCategoriesQuery(
    CategoryStatusEnum? Status = null
) : IRequest<IEnumerable<Category>>;
}
