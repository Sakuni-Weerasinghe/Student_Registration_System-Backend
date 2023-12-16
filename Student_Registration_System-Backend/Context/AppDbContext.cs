using Microsoft.EntityFrameworkCore;
using Student_Registration_System_Backend.Models;

namespace Student_Registration_System_Backend.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().ToTable("admins");

        }
    }
}
