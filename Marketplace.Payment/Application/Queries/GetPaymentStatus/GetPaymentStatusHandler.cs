using Marketplace.Payment.Domain.Enums;
using Marketplace.Payment.Infrastructure;
using MediatR;

namespace Marketplace.Payment.Application.Queries.GetPaymentStatus
{
    public class GetPaymentStatusHandler : IRequestHandler<GetPaymentStatusQuery, PaymentStatus>
    {
        private readonly PaymentDbContext _context;

        public GetPaymentStatusHandler(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentStatus> Handle(GetPaymentStatusQuery request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FindAsync(new object[] { request.PaymentId }, cancellationToken);

            if (payment == null)
                throw new ArgumentException("Payment not found");

            return payment.Status;
        }
    }
}
