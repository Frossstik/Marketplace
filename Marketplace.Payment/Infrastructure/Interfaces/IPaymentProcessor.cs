namespace Marketplace.Payment.Infrastructure.Interfaces
{
    public interface IPaymentProcessor
    {
        Task<string> ProcessPaymentAsync(
            Guid paymentId,
            decimal amount,
            string currency,
            Dictionary<string, string> paymentDetails,
            CancellationToken cancellationToken);

        Task<bool> ProcessRefundAsync(
            string transactionId,
            decimal amount,
            string currency,
            CancellationToken cancellationToken);
    }
}
