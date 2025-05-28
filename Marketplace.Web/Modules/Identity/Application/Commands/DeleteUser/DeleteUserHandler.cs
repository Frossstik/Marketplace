using Marketplace.Web.Modules.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Web.Modules.Identity.Application.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly UserManager<User> _userManager;

        public DeleteUserHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken token)
        {
            if (request.UserId == request.CurrentUserId)
                throw new ArgumentException("You cannot delete yourself");

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return false;

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
                throw new ArgumentException("Cannot delete admin users");

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
