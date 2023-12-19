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
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseSchedule> CourseSchedules { get; set; }
        public DbSet<Student_Courses> Student_Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().ToTable("admins");
            modelBuilder.Entity<Student>().ToTable("students");
            modelBuilder.Entity<Course>().ToTable("courses");
            modelBuilder.Entity<CourseSchedule>().ToTable("course_schedule");
            modelBuilder.Entity<Student_Courses>().ToTable("student_courses");


        }
    }
}
