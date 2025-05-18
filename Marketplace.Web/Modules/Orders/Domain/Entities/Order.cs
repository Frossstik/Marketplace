using Marketplace.Web.Modules.Orders.Domain.Enums;

namespace Marketplace.Web.Modules.Orders.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(i => i.UnitPrice * i.Quantity);
        public OrderStatus Status { get; set; } = OrderStatus.PendingPayment;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public void Cancel() => Status = OrderStatus.Cancelled;
    }
}
