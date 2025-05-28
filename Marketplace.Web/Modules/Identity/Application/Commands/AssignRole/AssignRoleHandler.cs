using Marketplace.Web.Modules.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Web.Modules.Identity.Application.Commands.AssignRole
{
    public class AssignRoleHandler : IRequestHandler<AssignRoleCommand>
    {
        private readonly UserManager<User> _userManager;

        public AssignRoleHandler(UserManager<User> userManager)
            => _userManager = userManager;

        public async Task<Unit> Handle(AssignRoleCommand command, CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(command.UserId.ToString());
            if (user == null)
                throw new ArgumentException("User not found");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, command.Role.ToString());

            return Unit.Value;
        }

        Task IRequestHandler<AssignRoleCommand>.Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }
}
