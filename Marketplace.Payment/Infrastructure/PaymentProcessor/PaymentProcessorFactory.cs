using Marketplace.Payment.Domain.Enums;
using Marketplace.Payment.Infrastructure.Interfaces;

namespace Marketplace.Payment.Infrastructure.PaymentProcessor
{
    public class PaymentProcessorFactory : IPaymentProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentProcessor GetProcessor(PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.Card => _serviceProvider.GetRequiredService<BankCardService>(),
                PaymentMethod.YooMoney => _serviceProvider.GetRequiredService<YooMoneyService>(),
                PaymentMethod.SBP => throw new NotImplementedException("SBP processor not implemented yet"),
                _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
            };
        }
    }
}
