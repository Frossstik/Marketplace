using Marketplace.Web.Modules.Products.Domain.Entities;
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
            builder.Property(p => p.ImagePaths).HasConversion(
                v => string.Join("|||", v),
                v => v.Split("|||", StringSplitOptions.RemoveEmptyEntries).ToList()
            );

            builder.Property(p => p.CreatorId).IsRequired(); 

            builder.Property(p => p.CreatorName).HasMaxLength(100);
            builder.HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId);
        }
    }
}