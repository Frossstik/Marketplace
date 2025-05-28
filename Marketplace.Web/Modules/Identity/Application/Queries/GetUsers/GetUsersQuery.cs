using MediatR;

namespace Marketplace.Web.Modules.Identity.Application.Queries.GetUsers
{

    public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;

}
