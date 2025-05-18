namespace Marketplace.Web.Modules.Carts.Domain.Entities
{
    public class CartItem
    {
        public Guid ProductId { get; }
        public string Name { get; }
        public decimal Price { get; }
        public int Quantity { get; set; }

        public CartItem(Guid productId, string name, decimal price, int quantity)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }

}
