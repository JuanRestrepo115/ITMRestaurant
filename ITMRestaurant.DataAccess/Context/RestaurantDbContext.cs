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
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<ReservationDetail> ReservationDetails => Set<ReservationDetail>();
        public DbSet<Restaurant> Restaurants => Set<Restaurant>();
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
            //---- MenuItem ----
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(m => m.Description)
                      .HasMaxLength(300);

                entity.Property(m => m.Price)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.Property(m => m.Category)
                      .IsRequired()
                      .HasConversion<int>();

                entity.Property(m => m.IsAvailable)
                      .IsRequired();

                entity.Property(m => m.CreatedAt)
                      .IsRequired();

                entity.Property(m => m.UpdatedAt)
                      .IsRequired(false);
            });
            //---- Reservation ----
            modelBuilder.Entity<Reservation>(entity =>
            { 
                entity.HasKey(r => r.Id);

                entity.Property(r => r.ReservationTime)
                      .IsRequired();

                entity.Property(r => r.NumberOfGuests)
                      .IsRequired();

                entity.Property(r => r.State)
                      .IsRequired()
                      .HasConversion<int>();

                entity.Property(r => r.Observations)
                      .HasMaxLength(500);

                entity.Property(r => r.CreatedAt)
                      .IsRequired();

                entity.Property(r => r.UpdatedAt)
                      .IsRequired(false);

                // Relacion con el Cliente 1:N
                entity.HasOne(r => r.Customer)
                      .WithMany(c => c.Reservations)
                      .HasForeignKey(r => r.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relacion con el Restaurante 1:N
                entity.HasOne(r => r.Restaurant)
                      .WithMany(res => res.Reservations)
                      .HasForeignKey(r => r.RestaurantId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relacion con la Mesa 1:N
                entity.HasOne(r => r.Table)
                      .WithMany(t => t.Reservations)
                      .HasForeignKey(r => r.TableId)
                      .OnDelete(DeleteBehavior.Restrict); // 👈 Restrict para evitar múltiples CASCADE
            });
            //---- ReservationDetail ----
            modelBuilder.Entity<ReservationDetail>(entity =>
            {
                entity.HasKey(rd => rd.Id);

                entity.Property(rd => rd.Quantity)
                      .IsRequired();

                entity.Property(rd => rd.UnitPrice)
                      .IsRequired()
                      .HasColumnType("decimal(10,2)");

                entity.Property(rd => rd.CreatedAt)
                      .IsRequired();

                entity.Property(rd => rd.UpdatedAt)
                      .IsRequired(false);

                // ReservationDetail N:1 Reservation
                entity.HasOne(rd => rd.Reservation)
                      .WithMany(r => r.ReservationDetails)
                      .HasForeignKey(rd => rd.ReservationId)
                      .OnDelete(DeleteBehavior.Cascade);

                // ReservationDetail N:1 MenuItem
                entity.HasOne(rd => rd.MenuItem)
                      .WithMany(m => m.ReservationDetails)
                      .HasForeignKey(rd => rd.MenuItemId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            //---- Restaurant ----
            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Branch)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(r => r.Address)
                      .IsRequired()
                      .HasMaxLength(250);

                entity.Property(r => r.PhoneNumber)
                      .HasMaxLength(20);

                entity.Property(r => r.IsActive)
                      .IsRequired();

                entity.Property(r => r.CreatedAt)
                      .IsRequired();

                entity.Property(r => r.UpdatedAt)
                      .IsRequired(false);

                // Restaurant 1:N Tables
                entity.HasMany(r => r.Tables)
                      .WithOne(t => t.Restaurant)
                      .HasForeignKey(t => t.RestaurantId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Restaurant 1:N Reservations
                entity.HasMany(r => r.Reservations)
                      .WithOne(res => res.Restaurant)
                      .HasForeignKey(res => res.RestaurantId)
                      .OnDelete(DeleteBehavior.Cascade);
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
