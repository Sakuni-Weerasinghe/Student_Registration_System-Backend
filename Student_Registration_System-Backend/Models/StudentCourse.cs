namespace Student_Registration_System_Backend.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public Student Student { get; set; }
        public Course Courses { get; set; }
    }
}
