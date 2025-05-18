using Marketplace.Payment.Domain.Entities;
using Marketplace.Payment.Domain.Enums;

namespace Marketplace.Payment.Presentation.GraphQL.Types
{
    public class PaymentInputType : InputObjectType<PaymentInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PaymentInput> descriptor)
        {
            descriptor.Field(i => i.OrderId).Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.Amount).Type<NonNullType<DecimalType>>();
            descriptor.Field(i => i.Currency).Type<NonNullType<StringType>>();
            descriptor.Field(i => i.Method).Type<NonNullType<EnumType<PaymentMethod>>>();
            descriptor.Field(i => i.PaymentDetails)
                .Type<NonNullType<AnyType>>()
                .Description("Key-value pairs of payment details");
        }
    }
}
