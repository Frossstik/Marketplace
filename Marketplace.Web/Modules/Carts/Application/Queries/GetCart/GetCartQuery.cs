using Marketplace.Web.Modules.Carts.Domain.Entities;
using MediatR;

namespace Marketplace.Web.Modules.Carts.Application.Queries.GetCart
{
    public record GetCartQuery(Guid UserId) : IRequest<Cart>;
}
