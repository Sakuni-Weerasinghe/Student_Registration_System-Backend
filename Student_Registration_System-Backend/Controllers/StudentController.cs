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
            registerstudentRequest.Email = registerstudentRequest.StudentRegistrationNumber + "@gmail.com";
            await _authContext.Students.AddAsync(registerstudentRequest);
            await _authContext.SaveChangesAsync();

            return Ok(registerstudentRequest);
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

    }
}