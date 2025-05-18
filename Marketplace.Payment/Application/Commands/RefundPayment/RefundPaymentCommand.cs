using MediatR;

namespace Marketplace.Payment.Application.Commands.RefundPayment
{
    public record RefundPaymentCommand(
        Guid PaymentId,
        decimal Amount) : IRequest<bool>;
}
