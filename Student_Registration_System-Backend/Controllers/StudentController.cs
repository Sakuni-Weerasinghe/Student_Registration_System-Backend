using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Registration_System_Backend.Context;
using Student_Registration_System_Backend.Helpers;
using Student_Registration_System_Backend.Models;

namespace Student_Registration_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        public StudentController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;
        }

        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent([FromBody] Student registerstudentRequest)
        {
            if (registerstudentRequest == null)
            {
                return BadRequest();
            }
            
            HelpingMethods.TrimStringProperties(registerstudentRequest);

            // Check if a student with the same registration number already exists
            if (_authContext.Students.Any(s => s.StudentRegistrationNumber == registerstudentRequest.StudentRegistrationNumber))
            {
                // Student with the same registration number already exists
                return BadRequest(new { Message = "A student with the same registration number already exists." });
            }
            registerstudentRequest.Email = registerstudentRequest.StudentRegistrationNumber + "@gmail.com";
            await _authContext.Students.AddAsync(registerstudentRequest);
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Student Registered!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var student = await _authContext.Students.ToListAsync();

            return Ok(student);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            var student = await _authContext.Students.FirstOrDefaultAsync(x => x.StudentId == id);
            if(student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPut]
        [Route("update/{studentId:int}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int studentId, Student updateStudentRequest)
        {
            var student = await _authContext.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound(new {Message = "Not Found"});
            }

            student.StudentRegistrationNumber = updateStudentRequest.StudentRegistrationNumber;
            student.FirstName = updateStudentRequest.FirstName;
            student.LastName = updateStudentRequest.LastName;
            student.Birthday = updateStudentRequest.Birthday;
            student.Gender = updateStudentRequest.Gender;
            student.Phone = updateStudentRequest.Phone;
            student.Email = updateStudentRequest.Email;
            student.AddressLine1 = updateStudentRequest.AddressLine1;
            student.AddressLine2 = updateStudentRequest.AddressLine2;
            student.AddressLine3 = updateStudentRequest.AddressLine3;

            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Sucessfully Updated!"
            });
        }

        [HttpDelete]
        [Route("delete/{studentId:int}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int studentId)
        {
            var student = await _authContext.Students
                .Include(s => s.StudentCourses)  // Include related courses to delete them as well
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                return NotFound();
            }

            if (student.StudentCourses.Any())
            {
                // Remove related courses
                _authContext.StudentCourses.RemoveRange(student.StudentCourses);
            }

            // Remove the student
            _authContext.Students.Remove(student);

            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Successfully Deleted" });

        }

    }
}