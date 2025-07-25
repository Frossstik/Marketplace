﻿using Marketplace.Payment.Application.Commands.ProcessPayment;
using Marketplace.Payment.Application.Commands.RefundPayment;
using Marketplace.Payment.Domain.Entities;
using Marketplace.Payment.Domain.Enums;
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
            var paymentDetails = new Dictionary<string, string>();

            if (input.Method == PaymentMethod.YooMoney && !string.IsNullOrEmpty(input.YooMoneyWallet))
            {
                paymentDetails["yoomoney_wallet"] = input.YooMoneyWallet;
            }
            else if (input.Method == PaymentMethod.Card && input.CardDetails != null)
            {
                paymentDetails["card_number"] = input.CardDetails.CardNumber;
                paymentDetails["expiry_date"] = input.CardDetails.ExpiryDate;
                paymentDetails["cvv"] = input.CardDetails.Cvv;
            }
            else
            {
                throw new Exception("Invalid payment details for the selected method.");
            }

            var command = new ProcessPaymentCommand(
                input.OrderId,
                input.Amount,
                input.Currency,
                input.Method,
                paymentDetails);

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
