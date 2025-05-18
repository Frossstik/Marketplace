using MediatR;

namespace Marketplace.Web.Modules.Categories.Application.Commands.ApproveCategory
{
    public record ApproveCategoryCommand(
    Guid CategoryId
) : IRequest;
}
