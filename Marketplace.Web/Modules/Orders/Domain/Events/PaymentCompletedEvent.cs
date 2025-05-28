namespace Marketplace.Web.Modules.Orders.Domain.Events
{
    namespace Marketplace.Web.Modules.Orders.Domain.Events
    {
        public class PaymentCompletedEvent
        {
            public Guid OrderId { get; set; }
            public Guid PaymentId { get; set; }
            public string TransactionId { get; set; } = null!;
            public decimal Amount { get; set; }
            public string Currency { get; set; }
        }
    }

}
