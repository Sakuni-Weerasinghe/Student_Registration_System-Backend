using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Student_Registration_System_Backend.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Lecturer { get; set; }

        [JsonIgnore]
        public ICollection<StudentCourse> StudentCourses { get; set; }





    }
}
