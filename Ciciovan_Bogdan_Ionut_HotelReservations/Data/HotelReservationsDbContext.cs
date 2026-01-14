using Microsoft.EntityFrameworkCore;
using Ciciovan_Bogdan_Ionut_HotelReservations.Models;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Data
{
    public class HotelReservationsDbContext : DbContext
    {
        public HotelReservationsDbContext(DbContextOptions<HotelReservationsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingHistory> BookingHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.HasKey(e => e.RoomTypeId);
                entity.Property(e => e.RoomTypeName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.BasePrice).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<MealPlan>(entity =>
            {
                entity.HasKey(e => e.MealPlanId);
                entity.Property(e => e.PlanName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PricePerPerson).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingId);
                entity.HasIndex(e => e.BookingCode).IsUnique();
                entity.Property(e => e.BookingCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.AvgPricePerRoom).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MarketSegmentType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.BookingStatus).IsRequired().HasMaxLength(20);

                entity.HasOne(b => b.Customer)
                    .WithMany(c => c.Bookings)
                    .HasForeignKey(b => b.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.RoomType)
                    .WithMany(r => r.Bookings)
                    .HasForeignKey(b => b.RoomTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.MealPlan)
                    .WithMany(m => m.Bookings)
                    .HasForeignKey(b => b.MealPlanId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<BookingHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId);
                entity.Property(e => e.ChangeType).IsRequired().HasMaxLength(50);

                entity.HasOne(h => h.Booking)
                    .WithMany(b => b.BookingHistories)
                    .HasForeignKey(h => h.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { RoomTypeId = 1, RoomTypeName = "Room Type 1", Description = "Standard Single Room", MaxOccupancy = 1, BasePrice = 50.00m, IsActive = true },
                new RoomType { RoomTypeId = 2, RoomTypeName = "Room Type 2", Description = "Standard Double Room", MaxOccupancy = 2, BasePrice = 75.00m, IsActive = true },
                new RoomType { RoomTypeId = 3, RoomTypeName = "Room Type 3", Description = "Deluxe Room", MaxOccupancy = 2, BasePrice = 100.00m, IsActive = true },
                new RoomType { RoomTypeId = 4, RoomTypeName = "Room Type 4", Description = "Family Room", MaxOccupancy = 4, BasePrice = 150.00m, IsActive = true },
                new RoomType { RoomTypeId = 5, RoomTypeName = "Room Type 5", Description = "Suite", MaxOccupancy = 3, BasePrice = 200.00m, IsActive = true },
                new RoomType { RoomTypeId = 6, RoomTypeName = "Room Type 6", Description = "Penthouse Suite", MaxOccupancy = 4, BasePrice = 300.00m, IsActive = true },
                new RoomType { RoomTypeId = 7, RoomTypeName = "Room Type 7", Description = "Presidential Suite", MaxOccupancy = 6, BasePrice = 500.00m, IsActive = true }
            );

            modelBuilder.Entity<MealPlan>().HasData(
                new MealPlan { MealPlanId = 1, PlanName = "Not Selected", Description = "No meals included", PricePerPerson = 0.00m, IsActive = true },
                new MealPlan { MealPlanId = 2, PlanName = "Meal Plan 1", Description = "Breakfast included", PricePerPerson = 15.00m, IsActive = true },
                new MealPlan { MealPlanId = 3, PlanName = "Meal Plan 2", Description = "Half Board (Breakfast + Dinner)", PricePerPerson = 35.00m, IsActive = true },
                new MealPlan { MealPlanId = 4, PlanName = "Meal Plan 3", Description = "Full Board (All meals)", PricePerPerson = 50.00m, IsActive = true }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "+40712345678",
                    IsRepeatedGuest = false,
                    TotalPreviousCancellations = 0,
                    TotalPreviousBookings = 0,
                    CreatedDate = new DateTime(2026, 1, 1, 10, 0, 0)
                },
                new Customer
                {
                    CustomerId = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "+40723456789",
                    IsRepeatedGuest = true,
                    TotalPreviousCancellations = 0,
                    TotalPreviousBookings = 0,
                    CreatedDate = new DateTime(2025, 6, 15, 14, 30, 0)
                }
            );
        }
    }
}