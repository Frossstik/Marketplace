using MediatR;

namespace Marketplace.Web.Modules.Categories.Application.Commands.DeleteEmptyCategory
{
    public record DeleteEmptyCategoryCommand(
    Guid CategoryId
) : IRequest<bool>;
}
