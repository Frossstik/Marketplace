using HotChocolate.Authorization;
using Marketplace.Web.Modules.Identity.Application.Commands.AssignRole;
using Marketplace.Web.Modules.Identity.Application.Commands.Register;
using Marketplace.Web.Modules.Identity.Application.DTO;
using Marketplace.Web.Modules.Identity.Domain.Enums;
using MediatR;
//using Microsoft.AspNetCore.Authorization;

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
    }
}
