using Marketplace.Payment.Domain.Enums;

namespace Marketplace.Payment.Infrastructure.Interfaces
{
    public interface IPaymentProcessorFactory
    {
        IPaymentProcessor GetProcessor(PaymentMethod method);
    }
}
