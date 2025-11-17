using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using Reservations.Api.Models;

namespace Reservations.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Place> Place { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<CoffeeOption> CoffeeOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Place>().HasMany(l => l.Rooms).WithOne(r => r.Place).HasForeignKey(r => r.PlaceId);
            modelBuilder.Entity<CoffeeOption>().HasOne(c => c.Reservation).WithOne(r => r.CoffeeOption).HasForeignKey<CoffeeOption>(c => c.ReservationId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
