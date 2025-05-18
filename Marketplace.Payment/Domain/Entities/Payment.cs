using Marketplace.Payment.Domain.Enums;

namespace Marketplace.Payment.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public string? TransactionId { get; private set; }
        public string? FailureReason { get; private set; }

        private Payment() { }

        public Payment(Guid orderId, decimal amount, string currency, PaymentMethod method)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
            Method = method;
            Status = PaymentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsCompleted(string transactionId)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Only pending payments can be completed");

            Status = PaymentStatus.Completed;
            TransactionId = transactionId ?? throw new ArgumentNullException(nameof(transactionId));
            ProcessedAt = DateTime.UtcNow;
        }

        public void MarkAsFailed(string reason)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Only pending payments can be failed");

            Status = PaymentStatus.Failed;
            FailureReason = reason ?? throw new ArgumentNullException(nameof(reason));
            ProcessedAt = DateTime.UtcNow;
        }
    }
}
