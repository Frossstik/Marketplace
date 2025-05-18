using Marketplace.Payment.Domain.Enums;
using MediatR;

namespace Marketplace.Payment.Application.Queries.GetPaymentStatus
{
    public record GetPaymentStatusQuery(Guid PaymentId) : IRequest<PaymentStatus>;
}
