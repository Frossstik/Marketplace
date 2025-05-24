using Marketplace.Payment.Infrastructure.Interfaces;

namespace Marketplace.Payment.Infrastructure.PaymentProcessor
{
    public class BankCardService : IPaymentProcessor
    {
        public async Task<string> ProcessPaymentAsync(
            Guid paymentId,
            decimal amount,
            string currency,
            Dictionary<string, string> paymentDetails,
            CancellationToken cancellationToken)
        {
            if (!paymentDetails.TryGetValue("card_number", out var cardNumber))
                throw new Exception("Card number is required");

            if (cardNumber.Length != 16 || !cardNumber.All(char.IsDigit))
                throw new Exception("Invalid card number format");

            await Task.Delay(1000, cancellationToken);

            return $"MOCK_CARD_{Guid.NewGuid()}";
        }

        public async Task<bool> ProcessRefundAsync(
            string transactionId,
            decimal amount,
            string currency,
            CancellationToken cancellationToken)
        {
            if (!transactionId.StartsWith("MOCK_CARD_"))
                throw new Exception("Invalid transaction ID");

            await Task.Delay(800, cancellationToken);
            return true;
        }
    }
}
