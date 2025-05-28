using Marketplace.Web.Modules.Identity.Application.DTO;
using Marketplace.Web.Modules.Identity.Application.Queries.Login;
using MediatR;
using Marketplace.Web.Modules.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using HotChocolate.Authorization;
using Marketplace.Web.Modules.Identity.Application.Queries.GetUserById;
using Marketplace.Web.Modules.Identity.Application.Queries.GetUsers;

namespace Marketplace.Web.Modules.Identity.Presentation.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class AuthQuery
    {
        [GraphQLDescription("Авторизация пользователя")]
        public async Task<AuthResponse> Login(
            [Service] IMediator mediator,
            LoginQuery input,
            CancellationToken token)
        {
            return await mediator.Send(input, token);
        }

        [Authorize]
        [GraphQLDescription("Получить данные текущего пользователя")]
        public async Task<User> Me(
            [Service] IHttpContextAccessor contextAccessor,
            [Service] UserManager<User> userManager)
        {
            var user = contextAccessor.HttpContext?.User;
            return await userManager.GetUserAsync(user!);
        }

        [GraphQLDescription("Получить URL для аутентификации через провайдера")]
        public string GetExternalLoginUrl(string provider)
        {
            return provider.ToLower() switch
            {
                "google" => "/signin-google",
                "yandex" => "/signin-yandex",
                _ => throw new ArgumentException("Неизвестный провайдер")
            };
        }

        [Authorize(Roles = new[] { "Admin" })]
        public async Task<User?> GetUserById(
            [Service] IMediator mediator,
            Guid userId,
            CancellationToken token)
        {
            return await mediator.Send(new GetUserByIdQuery(userId), token);
        }

        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<UserDto>> GetUsers(
            [Service] IMediator mediator,
            CancellationToken token)
        {
            return await mediator.Send(new GetUsersQuery(), token);
        }
    }
}
