using Marketplace.Payment.Application.Commands.ProcessPayment;
using Marketplace.Payment.Application.Commands.RefundPayment;
using Marketplace.Payment.Domain.Entities;
using MediatR;

namespace Marketplace.Payment.Presentation.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class PaymentMutations
    {
        public async Task<Domain.Entities.Payment> ProcessPayment(
            [Service] IMediator mediator,
            PaymentInput input)
        {
            var command = new ProcessPaymentCommand(
                input.OrderId,
                input.Amount,
                input.Currency,
                input.Method,
                input.PaymentDetails);

            return await mediator.Send(command);
        }

        public async Task<bool> RefundPayment(
            [Service] IMediator mediator,
            Guid paymentId,
            decimal amount)
        {
            var command = new RefundPaymentCommand(paymentId, amount);
            return await mediator.Send(command);
        }
    }
}
