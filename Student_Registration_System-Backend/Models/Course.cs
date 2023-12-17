using System.ComponentModel.DataAnnotations;

namespace Student_Registration_System_Backend.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }

        public int Credits { get; set; }
        public string Description { get; set; }
        public string Lecturer { get; set; }

  

    }
}
