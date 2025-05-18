using MediatR;

namespace Marketplace.Web.Modules.Categories.Application.Commands.CreateCategory
{
    public record CreateCategoryCommand(
    string Name,
    Guid? CreatorSellerId = null
) : IRequest<Guid>;
}
