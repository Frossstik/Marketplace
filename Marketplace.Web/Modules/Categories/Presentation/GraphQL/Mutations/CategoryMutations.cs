using HotChocolate.Authorization;
using Marketplace.Web.Modules.Categories.Application.Commands.ApproveCategory;
using Marketplace.Web.Modules.Categories.Application.Commands.CreateCategory;
using Marketplace.Web.Modules.Categories.Application.Commands.DeleteEmptyCategory;
using MediatR;

namespace Marketplace.Web.Modules.Categories.Presentation.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class CategoryMutations
    {
        [UseMutationConvention]
        public async Task<Guid> CreateCategory(
            [Service] IMediator mediator,
            CreateCategoryCommand input,
            CancellationToken token)
        {
            return await mediator.Send(input, token);
        }

        [Authorize(Roles = new[] { "Admin" })]
        public async Task<bool> ApproveCategory(
            [Service] IMediator mediator,
            Guid categoryId,
            CancellationToken token)
        {
            await mediator.Send(new ApproveCategoryCommand(categoryId), token);
            return true;
        }

        [Authorize(Roles = new[] { "Admin" })]
        public async Task<bool> DeleteEmptyCategory(
        [Service] IMediator mediator,
        Guid categoryId,
        CancellationToken token)
        {
            return await mediator.Send(new DeleteEmptyCategoryCommand(categoryId), token);
        }
    }
}
