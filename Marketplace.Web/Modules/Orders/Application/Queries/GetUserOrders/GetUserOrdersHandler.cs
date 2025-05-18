using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Orders.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Modules.Orders.Application.Queries.GetUserOrders
{
    public class GetUserOrdersHandler : IRequestHandler<GetUserOrdersQuery, IEnumerable<Order>>
    {
        private readonly AppDbContext _db;

        public GetUserOrdersHandler(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Order>> Handle(GetUserOrdersQuery query, CancellationToken token)
        {
            return await _db.Orders
                .Where(o => o.UserId == query.UserId)
                .Include(o => o.Items)
                .AsNoTracking()
                .ToListAsync(token);
        }
    }
}
