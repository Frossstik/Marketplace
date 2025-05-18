using Marketplace.Web.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Modules.Orders.Application.Commands.CancelOrder
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly AppDbContext _db;

        public CancelOrderHandler(AppDbContext db) => _db = db;

        public async Task Handle(CancelOrderCommand command, CancellationToken token)
        {
            var order = await _db.Orders
                .FirstOrDefaultAsync(o => o.Id == command.OrderId && o.UserId == command.UserId, token);

            if (order is null)
                throw new Exception("Order not found");

            order.Cancel();
            await _db.SaveChangesAsync(token);
        }
    }
}
