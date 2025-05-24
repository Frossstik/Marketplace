using Marketplace.Payment.Infrastructure.Interfaces;

namespace Marketplace.Payment.Infrastructure.PaymentProcessor
{
    public class YooMoneyService : IPaymentProcessor
    {
        public async Task<string> ProcessPaymentAsync(
            Guid paymentId,
            decimal amount,
            string currency,
            Dictionary<string, string> paymentDetails,
            CancellationToken cancellationToken)
        {
            if (!paymentDetails.TryGetValue("yoomoney_wallet", out var wallet))
                throw new Exception("YooMoney wallet is required");

            await Task.Delay(1500, cancellationToken);
            return $"MOCK_YM_{Guid.NewGuid()}";
        }

        public async Task<bool> ProcessRefundAsync(
            string transactionId,
            decimal amount,
            string currency,
            CancellationToken cancellationToken)
        {
            if (!transactionId.StartsWith("MOCK_YM_"))
                throw new Exception("Invalid transaction ID");

            await Task.Delay(1000, cancellationToken);
            return true;
        }
    }

}
