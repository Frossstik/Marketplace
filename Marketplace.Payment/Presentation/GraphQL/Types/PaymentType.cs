using Marketplace.Payment.Domain.Enums;

namespace Marketplace.Payment.Presentation.GraphQL.Types
{
    public class PaymentType : ObjectType<Domain.Entities.Payment>
    {
        protected override void Configure(IObjectTypeDescriptor<Domain.Entities.Payment> descriptor)
        {
            descriptor.Field(p => p.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(p => p.OrderId).Type<NonNullType<UuidType>>();
            descriptor.Field(p => p.Amount).Type<NonNullType<DecimalType>>();
            descriptor.Field(p => p.Currency).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.Method).Type<NonNullType<EnumType<PaymentMethod>>>();
            descriptor.Field(p => p.Status).Type<NonNullType<EnumType<PaymentStatus>>>();
            descriptor.Field(p => p.CreatedAt).Type<NonNullType<DateTimeType>>();
            descriptor.Field(p => p.ProcessedAt).Type<DateTimeType>();
            descriptor.Field(p => p.TransactionId).Type<StringType>();
            descriptor.Field(p => p.FailureReason).Type<StringType>();
        }
    }
}
