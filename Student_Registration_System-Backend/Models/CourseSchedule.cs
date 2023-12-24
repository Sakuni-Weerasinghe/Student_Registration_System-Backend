using System.ComponentModel.DataAnnotations;

namespace Student_Registration_System_Backend.Models
{
    public class CourseSchedule
    {
        [Key]
        public int CourseScheduleId { get; set; }
        public int CourseId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Venue {  get; set; }

        public Course Courses { get; set; }

    }
}
