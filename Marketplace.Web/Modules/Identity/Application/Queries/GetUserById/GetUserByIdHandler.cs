using Marketplace.Web.Modules.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Web.Modules.Identity.Application.Queries.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User?>
    {
        private readonly UserManager<User> _userManager;

        public GetUserByIdHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken token)
        {
            return await _userManager.FindByIdAsync(request.UserId.ToString());
        }
    }
}
