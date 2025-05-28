using Marketplace.Web.Modules.Identity.Application.DTO;
using Marketplace.Web.Modules.Identity.Application.Interfaces;
using Marketplace.Web.Modules.Identity.Domain.Entities;
using Marketplace.Web.Modules.Identity.Domain.Enums;
using Marketplace.Web.Modules.Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Web.Modules.Identity.Application.Queries.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public LoginHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new ArgumentException("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                throw new ArgumentException("Invalid credentials");

            var token = await _tokenService.GenerateToken(user);
            Console.WriteLine($"Generated token for {request.Email}: {token}");

            var roles = await _userManager.GetRolesAsync(user);
            var roleEnum = Enum.Parse<RoleEnum>(roles.First());

            return new AuthResponse(
                token,
                user.Id,
                user.Email,
                roleEnum
            );
        }
    }
}
