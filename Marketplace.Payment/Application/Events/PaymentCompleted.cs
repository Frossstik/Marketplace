namespace Marketplace.Payment.Application.Events
{
    public record PaymentCompleted(
        Guid PaymentId,
        Guid OrderId);
}
