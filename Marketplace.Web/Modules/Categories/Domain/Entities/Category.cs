using Marketplace.Web.Modules.Categories.Domain.Enums;

namespace Marketplace.Web.Modules.Categories.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? CreatorSellerId { get; set; } // null если создана системой/админом
        public CategoryStatusEnum? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
