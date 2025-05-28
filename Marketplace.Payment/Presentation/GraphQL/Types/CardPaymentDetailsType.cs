using Marketplace.Payment.Domain.Entities;

namespace Marketplace.Payment.Presentation.GraphQL.Types
{
    public class CardPaymentDetailsType : InputObjectType<CardPaymentDetails>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CardPaymentDetails> descriptor)
        {
            descriptor.Field(d => d.CardNumber).Type<NonNullType<StringType>>().Name("card_number");
            descriptor.Field(d => d.ExpiryDate).Type<NonNullType<StringType>>().Name("expiry_date");
            descriptor.Field(d => d.Cvv).Type<NonNullType<StringType>>().Name("cvv");
        }
    }
}
