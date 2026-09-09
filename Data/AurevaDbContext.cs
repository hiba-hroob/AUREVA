using AUREVA.Models;
using Microsoft.EntityFrameworkCore;

namespace AUREVA.Data
{
    public class AurevaDbContext : DbContext
    {
        public AurevaDbContext(DbContextOptions<AurevaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Appointment → Client
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Client)
                .WithMany()
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment → Service
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment → Staff
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Staff)
                .WithMany()
                .HasForeignKey(a => a.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal precision
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Client>()
                .Property(c => c.TotalSpent)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Client>()
                .Property(c => c.Rating)
                .HasPrecision(3, 1);
        }
    }
}