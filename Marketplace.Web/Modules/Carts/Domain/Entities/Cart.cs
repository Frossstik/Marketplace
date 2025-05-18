namespace Marketplace.Web.Modules.Carts.Domain.Entities
{
    public class Cart
    {
        public Guid UserId { get; }
        public List<CartItem> Items { get; } = new();
        public decimal Total => Items.Sum(i => i.Price * i.Quantity);

        public Cart(Guid userId) => UserId = userId;

        public void AddItem(CartItem item)
        {
            var existing = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existing != null)
                existing.Quantity += item.Quantity;
            else
                Items.Add(item);
        }

        public void RemoveItem(Guid productId, int quantity = 1)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return;

            item.Quantity -= quantity;
            if (item.Quantity <= 0)
                Items.Remove(item);
        }

        public void Clear() => Items.Clear();
    }
}
