using Marketplace.Payment.Application.Events;
using Marketplace.Payment.Infrastructure;
using Marketplace.Payment.Infrastructure.Interfaces;
using MediatR;

namespace Marketplace.Payment.Application.Commands.ProcessPayment
{
    public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, Domain.Entities.Payment>
    {
        private readonly PaymentDbContext _context;
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;

        public ProcessPaymentHandler(
            PaymentDbContext context,
            IPaymentProcessorFactory paymentProcessorFactory)
        {
            _context = context;
            _paymentProcessorFactory = paymentProcessorFactory;
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
