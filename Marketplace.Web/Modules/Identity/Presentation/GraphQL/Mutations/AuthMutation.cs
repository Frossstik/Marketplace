using HotChocolate.Authorization;
using Marketplace.Web.Modules.Identity.Application.Commands.AssignRole;
using Marketplace.Web.Modules.Identity.Application.Commands.DeleteUser;
using Marketplace.Web.Modules.Identity.Application.Commands.ExternalLogin;
using Marketplace.Web.Modules.Identity.Application.Commands.Register;
using Marketplace.Web.Modules.Identity.Application.DTO;
using Marketplace.Web.Modules.Identity.Domain.Entities;
using Marketplace.Web.Modules.Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Web.Modules.Identity.Presentation.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class AuthMutation
    {
        [GraphQLDescription("Регистрация нового пользователя")]
        public async Task<AuthResponse> Register(
            [Service] IMediator mediator,
            RegisterCommand input,
            CancellationToken token)
        {
            return await mediator.Send(input, token);
        }

        [Authorize(Roles = new[] { "Admin" })]
        [GraphQLDescription("Назначить роль пользователю (только для админов)")]
        public async Task<bool> AssignRole(
            [Service] IMediator mediator,
            AssignRoleCommand input,
            CancellationToken token)
        {
            await mediator.Send(input, token);
            return true;
        }

        [GraphQLDescription("Вход через внешнего провайдера (Google/Yandex)")]
        public async Task<AuthResponse> ExternalLogin(
            [Service] IMediator mediator,
            string email,
            string firstName,
            string lastName,
            string provider,
            string? companyName = null,
            CancellationToken token = default)
        {
            return await mediator.Send(
                new ExternalLoginCommand(email, firstName, lastName, companyName, provider),
                token
            );
        }

        [Authorize]
        public async Task<bool> DeleteUser(
            [Service] IMediator mediator,
            [Service] IHttpContextAccessor httpContextAccessor,
            [Service] UserManager<User> userManager,
            Guid? userId = null,
            CancellationToken token = default)
        {
            var currentUser = httpContextAccessor.HttpContext?.User;
            var currentUserId = (await userManager.GetUserAsync(currentUser!))?.Id;

            var userIdToDelete = userId ?? currentUserId;
            if (!userIdToDelete.HasValue)
                throw new ArgumentException("User not found");

            return await mediator.Send(
                new DeleteUserCommand(userIdToDelete.Value, currentUserId!.Value),
                token);
        }
    }
}
