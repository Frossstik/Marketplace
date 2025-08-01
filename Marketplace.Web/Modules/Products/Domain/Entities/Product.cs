﻿using Marketplace.Web.Modules.Categories.Domain.Entities;
using Marketplace.Web.Modules.Identity.Domain.Entities;

namespace Marketplace.Web.Modules.Products.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public List<string> ImagePaths { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public Guid CreatorId { get; set; }
        public string? CreatorName { get; set; }
        public string? CreatorsCompanyName { get; set; } 
    }
}
