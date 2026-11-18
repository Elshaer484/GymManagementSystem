using GymManagment.DAL.Data.Configurations;
using GymManagment.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagment.DAL.Data.DbContexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {

        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //        "Server=(localdb)\\MSSQLLocalDB;Database=GymManagementDB;Trusted_Connection=True;TrustServerCertificate=True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ApplicationUser>(eb =>
            {
                eb.Property(x => x.FirstName)
                 .HasColumnType("varchar")
                 .HasMaxLength(50);

                eb.Property(x => x.LastName)
                 .HasColumnType("varchar")
                 .HasMaxLength(50);
            });
        }
        public DbSet<Plan> plans { get; set; } 
        public DbSet<Member> members { get; set; } 
        public DbSet<MemberShip> memberships { get; set; } 
        public DbSet<Session> sessions { get; set; } 
        public DbSet<Category> categories { get; set; } 
        public DbSet<Trainer> trainers { get; set; } 
        public DbSet<HealthRecord> healthRecords { get; set; } 
        public DbSet<Booking> bookings { get; set; } 

    }
}

// ctrl + d (copy)
