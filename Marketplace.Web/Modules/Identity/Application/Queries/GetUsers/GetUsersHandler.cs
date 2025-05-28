using Marketplace.Web.Modules.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Marketplace.Web.Modules.Identity.Application.Queries.GetUsers.GetUsersQuery;

namespace Marketplace.Web.Modules.Identity.Application.Queries.GetUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly UserManager<User> _userManager;

        public GetUsersHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.ToList();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CompanyName = user.CompanyName,
                    CreatedAt = user.CreatedAt,
                    Roles = roles
                });
            }

            return userDtos;
        }
    }

}
