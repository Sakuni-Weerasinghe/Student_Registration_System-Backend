using System.ComponentModel.DataAnnotations;

namespace Student_Registration_System_Backend.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Birthday { get; set; }
        public string Gender { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set;}
        public string AddressLine3 { get; set;}

    }
}
