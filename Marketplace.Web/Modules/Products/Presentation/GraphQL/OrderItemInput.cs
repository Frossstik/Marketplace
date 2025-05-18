namespace Marketplace.Web.Modules.Products.Presentation.GraphQL
{
    public record OrderItemInput(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity);
}
