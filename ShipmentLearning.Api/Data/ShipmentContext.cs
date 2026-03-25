using Microsoft.EntityFrameworkCore;
using ShipmentLearning;

namespace ShipmentLearning.Api.Data
{
    public class ShipmentContext : DbContext
    {
        public ShipmentContext(DbContextOptions<ShipmentContext> options)
            : base(options)
        {
        }

        public DbSet<Parcel> Parcels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parcel>().HasKey(p => p.Id);
            modelBuilder.Entity<Parcel>()
                .Property(p => p.Category)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
