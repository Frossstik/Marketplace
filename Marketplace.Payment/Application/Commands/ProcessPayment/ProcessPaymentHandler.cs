using Marketplace.Payment.Domain.Events;
using Marketplace.Payment.Infrastructure;
using Marketplace.Payment.Infrastructure.Interfaces;
using MediatR;

namespace Marketplace.Payment.Application.Commands.ProcessPayment
{
    public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, Domain.Entities.Payment>
    {
        private readonly PaymentDbContext _context;
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;
        private readonly IMessageBusPublisher _messageBus;

        public ProcessPaymentHandler(
            PaymentDbContext context,
            IPaymentProcessorFactory paymentProcessorFactory,
            IMessageBusPublisher messageBus)
        {
            _context = context;
            _paymentProcessorFactory = paymentProcessorFactory;
            _messageBus = messageBus;
        }

        public async Task<Domain.Entities.Payment> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Domain.Entities.Payment(
                request.OrderId,
                request.Amount,
                request.Currency,
                request.Method);

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);

            try
            {
                var processor = _paymentProcessorFactory.GetProcessor(request.Method);

                var transactionId = await processor.ProcessPaymentAsync(
                    payment.Id,
                    request.Amount,
                    request.Currency,
                    request.PaymentDetails,
                    cancellationToken);

                payment.MarkAsCompleted(transactionId);
                await _context.SaveChangesAsync(cancellationToken);

                var @event = new PaymentCompletedEvent(
                    payment.OrderId,
                    payment.Id,
                    payment.TransactionId!,
                    payment.Amount,
                    payment.Currency
                );

                await _messageBus.PublishAsync(@event, cancellationToken);

                return payment;
            }
            catch (Exception ex)
            {
                payment.MarkAsFailed(ex.Message);
                await _context.SaveChangesAsync(cancellationToken);
                throw;
            }
        }
    }
}
