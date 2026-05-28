using Microsoft.EntityFrameworkCore;
using ITMRestaurant.Domain.Entities;

namespace ITMRestaurant.DataAccess.Context
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Table> Tables => Set<Table>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //---- Customers Building ----
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.FirstName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.LastName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Email)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.HasIndex(c => c.Email)
                      .IsUnique();

                entity.Property(c => c.PhoneNumber)
                      .HasMaxLength(20);

                entity.Property(c => c.CreatedAt)
                      .IsRequired();

                entity.Property(c => c.UpdatedAt)
                      .IsRequired(false);
            });
            //---- Tables Building ----
            modelBuilder.Entity<Table>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.TableNumber)
                      .IsRequired();

                entity.HasIndex(t => t.TableNumber)
                      .IsUnique();

                entity.Property(t => t.Capacity)
                      .IsRequired();

                entity.Property(t => t.Location)
                      .HasMaxLength(100);

                entity.Property(t => t.State)
                      .IsRequired()
                      .HasConversion<int>();

                entity.Property(t => t.CreatedAt)
                      .IsRequired();

                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false);
            });

        }





    }
}
