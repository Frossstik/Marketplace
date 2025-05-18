using Marketplace.Web.Modules.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Web.Modules.Orders.Domain.Configurations
{
    public class OrderConfiguration
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Status)
                .HasConversion<string>();

            builder.OwnsMany(o => o.Items, item =>
            {
                item.ToTable("order_items");
                item.WithOwner().HasForeignKey("OrderId");
                item.Property(i => i.ProductId).IsRequired();
                item.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
            });
        }
    }
}
