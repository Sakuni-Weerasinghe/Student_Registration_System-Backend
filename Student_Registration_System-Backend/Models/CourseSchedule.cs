using System.ComponentModel.DataAnnotations;

namespace Student_Registration_System_Backend.Models
{
    public class CourseSchedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public string CourseCode { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }
        public string Venue {  get; set; }

    }
}
