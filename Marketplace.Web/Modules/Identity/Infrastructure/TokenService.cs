using Marketplace.Web.Modules.Identity.Application.Interfaces;
using Marketplace.Web.Modules.Identity.Domain.Entities;
using Marketplace.Web.Modules.Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Marketplace.Web.Modules.Identity.Infrastructure
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;

        public TokenService(
            IConfiguration config,
            UserManager<User> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleEnum = Enum.Parse<RoleEnum>(roles.First());

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, roles.First()),
                new(ClaimTypes.Role, roleEnum.ToString()),
                new Claim(ClaimTypes.Role, "Seller"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:ExpiryInMinutes")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}