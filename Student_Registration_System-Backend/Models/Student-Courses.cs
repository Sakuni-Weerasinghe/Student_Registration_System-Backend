using System.ComponentModel.DataAnnotations;

namespace Student_Registration_System_Backend.Models
{
    public class Student_Courses
    {
        [Key]
        public int StudentCourseId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

    }
}
