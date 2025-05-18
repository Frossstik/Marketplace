using Microsoft.EntityFrameworkCore;

namespace Marketplace.Payment.Infrastructure
{
    public class PaymentDbContext : DbContext
    {
        public DbSet<Domain.Entities.Payment> Payments { get; set; }

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Payment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsFixedLength();
            });
        }
    }
}
