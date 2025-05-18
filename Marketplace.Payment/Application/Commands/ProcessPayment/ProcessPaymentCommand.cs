using Marketplace.Payment.Domain.Enums;
using MediatR;

namespace Marketplace.Payment.Application.Commands.ProcessPayment
{
    public record ProcessPaymentCommand(
        Guid OrderId,
        decimal Amount,
        string Currency,
        PaymentMethod Method,
        Dictionary<string, string> PaymentDetails) : IRequest<Domain.Entities.Payment>;
}
