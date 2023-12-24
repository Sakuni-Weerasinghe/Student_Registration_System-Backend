using Microsoft.EntityFrameworkCore;
using Student_Registration_System_Backend.Models;
using System.Reflection.Metadata;

namespace Student_Registration_System_Backend.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<CourseSchedule> CourseSchedules { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().ToTable("admins");
            modelBuilder.Entity<Student>().ToTable("students");
            modelBuilder.Entity<Course>().ToTable("courses");

            // Configure many-to-many relationship
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Courses)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<CourseSchedule>()
                .HasOne(sc => sc.Courses)
                .WithMany(c => c.CourseSchedules)
                .HasForeignKey(cs => cs.CourseId);


        }
    }
}
