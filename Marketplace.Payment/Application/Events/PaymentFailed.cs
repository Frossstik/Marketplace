namespace Marketplace.Payment.Application.Events
{
    public record PaymentFailed(
        Guid PaymentId,
        Guid OrderId,
        string Reason);
}
