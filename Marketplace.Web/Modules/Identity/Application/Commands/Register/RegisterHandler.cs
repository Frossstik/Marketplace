using Marketplace.Web.Modules.Identity.Application.DTO;
using Marketplace.Web.Modules.Identity.Application.Interfaces;
using Marketplace.Web.Modules.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Web.Modules.Identity.Application.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public RegisterHandler(
            UserManager<User> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> Handle(RegisterCommand command, CancellationToken token)
        {
            var user = new User
            {
                Email = command.Email,
                UserName = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                CompanyName = command.CompanyName
            };

            var result = await _userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
                throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));

            // Назначаем роль (используем enum)
            await _userManager.AddToRoleAsync(user, command.Role.ToString());

            return new AuthResponse(
                await _tokenService.GenerateToken(user),
                user.Id,
                user.Email,
                command.Role
            );
        }
    }
}
