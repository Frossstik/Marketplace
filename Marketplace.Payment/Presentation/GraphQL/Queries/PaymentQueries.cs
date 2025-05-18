using Marketplace.Payment.Application.Queries.GetPaymentStatus;
using Marketplace.Payment.Domain.Enums;
using MediatR;

namespace Marketplace.Payment.Presentation.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class PaymentQueries
    {
        public async Task<PaymentStatus> GetPaymentStatus(
            [Service] IMediator mediator,
            Guid paymentId)
        {
            return await mediator.Send(new GetPaymentStatusQuery(paymentId));
        }
    }
}
