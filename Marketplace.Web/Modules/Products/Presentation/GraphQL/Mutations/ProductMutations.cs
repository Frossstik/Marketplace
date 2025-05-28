using HotChocolate.Authorization;
using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Identity.Domain.Entities;
using Marketplace.Web.Modules.Products.Application.Commands.CreateProduct;
using Marketplace.Web.Modules.Products.Application.Commands.DeleteProduct;
using Marketplace.Web.Modules.Products.Application.Commands.UpdateProduct;
using Marketplace.Web.Modules.Products.Application.Queries.GetProductById;
using Marketplace.Web.Modules.Products.Domain.Entities;
using Marketplace.Web.Modules.Products.Presentation.GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Marketplace.Web.Modules.Products.Presentation.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class ProductMutations
    {
        [Authorize(Roles = new[] { "Admin", "Seller" })]
        public async Task<Product> CreateProduct(
            [GraphQLType(typeof(ProductInputType))] CreateProductCommand input,
            [Service] IMediator mediator,
            [Service] IHttpContextAccessor httpContextAccessor,
            [Service] UserManager<User> userManager,
            CancellationToken cancellationToken)
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user == null) throw new UnauthorizedAccessException();

            var isAuthorized = user.IsInRole("Admin") || user.IsInRole("Seller");
            if (!isAuthorized) throw new UnauthorizedAccessException();

            return await mediator.Send(input, cancellationToken);
        }

        [Authorize(Roles = new[] { "Admin", "Seller" })]
        public async Task<Product> UpdateProduct(
            [GraphQLType(typeof(UpdateProductInputType))] UpdateProductCommand input,
            [Service] IMediator mediator,
            [Service] IHttpContextAccessor httpContextAccessor,
            [Service] UserManager<User> userManager,
            CancellationToken cancellationToken)
        {
            var currentUser = httpContextAccessor.HttpContext?.User;
            var user = await userManager.GetUserAsync(currentUser!);

            if (!await userManager.IsInRoleAsync(user!, "Admin"))
            {
                var product = await mediator.Send(new GetProductByIdQuery(input.Id), cancellationToken);
                if (product?.CreatorId != user?.Id)
                {
                    throw new UnauthorizedAccessException("You can only update your own products");
                }
            }

            return await mediator.Send(input, cancellationToken);
        }

        [Authorize(Roles = new[] { "Admin", "Seller" })]
        public async Task<bool> DeleteProduct(
            [ID] Guid id,
            [Service] IMediator mediator,
            [Service] IHttpContextAccessor httpContextAccessor,
            [Service] UserManager<User> userManager,
            CancellationToken cancellationToken)
        {
            var currentUser = httpContextAccessor.HttpContext?.User;
            var user = await userManager.GetUserAsync(currentUser!);

            if (!await userManager.IsInRoleAsync(user!, "Admin"))
            {
                var product = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);
                if (product?.CreatorId != user?.Id)
                {
                    throw new UnauthorizedAccessException("You can only delete your own products");
                }
            }

            return await mediator.Send(new DeleteProductCommand(id), cancellationToken);
        }
    }
}
