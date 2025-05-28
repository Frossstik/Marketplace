namespace Marketplace.Web.Modules.Categories.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? CreatorSellerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
