﻿namespace Marketplace.Web.Modules.Orders.Domain.Entities
{
    public class OrderItem
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
