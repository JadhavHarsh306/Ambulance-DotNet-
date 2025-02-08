using AmbulanceService.Model;
using AmbulanceService.Models;
using Microsoft.EntityFrameworkCore;

namespace AmbulanceService.DAL
{
    public class AmbulanceDBContext : DbContext
    {
        public AmbulanceDBContext(DbContextOptions<AmbulanceDBContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Driver> drivers { get; set; }
        public DbSet<Admin> admins { get; set; }
        public DbSet<Ambulance> ambulances { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<Hospital> hospitals { get; set; }

        public DbSet<Payments> payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Payments>()
                .Property(p => p.PaymentStatus)
                .HasConversion<string>();
            modelBuilder.Entity<Payments>().ToTable("Payments");
        }



    }
}
