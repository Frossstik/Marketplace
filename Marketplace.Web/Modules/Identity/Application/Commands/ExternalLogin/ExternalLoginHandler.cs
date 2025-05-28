using Marketplace.Web.Modules.Identity.Application.DTO;
using Marketplace.Web.Modules.Identity.Application.Interfaces;
using Marketplace.Web.Modules.Identity.Domain.Entities;
using Marketplace.Web.Modules.Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Web.Modules.Identity.Application.Commands.ExternalLogin
{
    public class ExternalLoginHandler : IRequestHandler<ExternalLoginCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public ExternalLoginHandler(
            UserManager<User> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = request.Email,
                    UserName = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CompanyName = request.CompanyName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                    throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));

                await _userManager.AddToRoleAsync(user, RoleEnum.Client.ToString());
            }

            return new AuthResponse(
                await _tokenService.GenerateToken(user),
                user.Id,
                user.Email,
                (await _userManager.GetRolesAsync(user)).First().ToRoleEnum()
            );
        }
    }
}