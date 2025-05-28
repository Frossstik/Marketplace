using Marketplace.Web.Modules.Identity.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Identity.Application.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<User?>;
}
