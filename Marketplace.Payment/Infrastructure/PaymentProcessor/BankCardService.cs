using Marketplace.Payment.Infrastructure.Interfaces;

namespace Marketplace.Payment.Infrastructure.PaymentProcessor
{
    public class BankCardService : IPaymentProcessor
    {
        public Task<string> ProcessPaymentAsync(
            Guid paymentId,
            decimal amount,
            string currency,
            Dictionary<string, string> paymentDetails,
            CancellationToken cancellationToken)
        {
            // Здесь должна быть реальная интеграция с платежным шлюзом
            // Например, Stripe, PayPal, или другим провайдером

            // Для примера просто возвращаем фейковый transactionId
            var transactionId = $"CARD_{Guid.NewGuid()}";
            return Task.FromResult(transactionId);
        }

        public Task<bool> ProcessRefundAsync(
            string transactionId,
            decimal amount,
            string currency,
            CancellationToken cancellationToken)
        {
            // Реальная логика возврата средств
            return Task.FromResult(true);
        }
    }
}
