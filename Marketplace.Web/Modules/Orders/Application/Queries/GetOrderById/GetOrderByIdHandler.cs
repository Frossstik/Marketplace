using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Orders.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Modules.Orders.Application.Queries.GetOrderById
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly AppDbContext _db;

        public GetOrderByIdHandler(AppDbContext db) => _db = db;

        public async Task<Order> Handle(GetOrderByIdQuery query, CancellationToken token)
        {
            return await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == query.OrderId && o.UserId == query.UserId, token)
                ?? throw new Exception("Order not found");
        }
    }
}
