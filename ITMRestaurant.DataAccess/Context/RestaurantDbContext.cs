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


        //---- Customers Building ----
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }




    }
}
