using Marketplace.Web.Modules.Categories.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Categories.Application.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(
    Guid CategoryId
) : IRequest<Category?>;
}
