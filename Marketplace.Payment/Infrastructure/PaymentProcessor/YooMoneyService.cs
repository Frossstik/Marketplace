using Marketplace.Payment.Infrastructure.Interfaces;

namespace Marketplace.Payment.Infrastructure.PaymentProcessor
{
    public class YooMoneyService : IPaymentProcessor
    {
        public Task<string> ProcessPaymentAsync(
            Guid paymentId,
            decimal amount,
            string currency,
            Dictionary<string, string> paymentDetails,
            CancellationToken cancellationToken)
        {
            // Интеграция с API ЮMoney

            // Для примера просто возвращаем фейковый transactionId
            var transactionId = $"YOOMONEY_{Guid.NewGuid()}";
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
