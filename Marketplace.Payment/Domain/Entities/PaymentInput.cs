using Marketplace.Payment.Domain.Enums;

namespace Marketplace.Payment.Domain.Entities
{
    public class PaymentInput
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "RUB";
        public PaymentMethod Method { get; set; }
        public CardPaymentDetails CardDetails { get; set; }
        public string YooMoneyWallet { get; set; }
    }
}
