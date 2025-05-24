using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Web.Modules.Products.Domain.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

            // Хранение списка как JSON
            builder.Property(p => p.ImagePaths)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, null) ?? new List<string>()
                );
        }
    }
}