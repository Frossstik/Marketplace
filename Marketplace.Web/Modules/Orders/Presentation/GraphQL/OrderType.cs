using Marketplace.Web.Modules.Orders.Domain.Entities;

namespace Marketplace.Web.Modules.Orders.Presentation.GraphQL
{
    public class OrderType : ObjectType<Order>
    {
        protected override void Configure(IObjectTypeDescriptor<Order> descriptor)
        {
            descriptor.Field(o => o.Id).Type<IdType>();
            descriptor.Field(o => o.TotalPrice).Type<DecimalType>();
            descriptor.Field(o => o.Status).Type<StringType>();
        }
    }
}
