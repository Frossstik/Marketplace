namespace Marketplace.Web.Modules.Orders.Domain.Events
{
    public sealed record OrderCreatedEvent(
    Guid OrderId,
    decimal TotalPrice
) : DomainEvent;
}
