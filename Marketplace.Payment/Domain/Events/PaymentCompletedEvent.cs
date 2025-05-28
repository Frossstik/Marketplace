namespace Marketplace.Payment.Domain.Events
{
    public class PaymentCompletedEvent
    {
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public PaymentCompletedEvent(Guid orderId, Guid paymentId, string transactionId, decimal amount, string currency)
        {
            OrderId = orderId;
            PaymentId = paymentId;
            TransactionId = transactionId;
            Amount = amount;
            Currency = currency;
        }
    }
}
