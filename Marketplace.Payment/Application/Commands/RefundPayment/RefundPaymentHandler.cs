using Marketplace.Payment.Domain.Enums;
using Marketplace.Payment.Infrastructure;
using Marketplace.Payment.Infrastructure.Interfaces;
using MediatR;

namespace Marketplace.Payment.Application.Commands.RefundPayment
{
    public class RefundPaymentHandler : IRequestHandler<RefundPaymentCommand, bool>
    {
        private readonly PaymentDbContext _context;
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;

        public RefundPaymentHandler(
            PaymentDbContext context,
            IPaymentProcessorFactory paymentProcessorFactory)
        {
            _context = context;
            _paymentProcessorFactory = paymentProcessorFactory;
        }

        public async Task<bool> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FindAsync(new object[] { request.PaymentId }, cancellationToken);
            if (payment == null)
                throw new ArgumentException("Payment not found");

            if (payment.Status != PaymentStatus.Completed)
                throw new InvalidOperationException("Only completed payments can be refunded");

            var processor = _paymentProcessorFactory.GetProcessor(payment.Method);
            var success = await processor.ProcessRefundAsync(
                payment.TransactionId!,
                request.Amount,
                payment.Currency,
                cancellationToken);

            if (success)
            {
                payment.MarkAsRefunded();
                await _context.SaveChangesAsync(cancellationToken);
            }

            return success;
        }
    }
}
