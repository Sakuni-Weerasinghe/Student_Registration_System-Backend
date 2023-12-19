using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Registration_System_Backend.Context;
using Student_Registration_System_Backend.Models;

namespace Student_Registration_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCoursesController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        public StudentCoursesController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;
        }

        [HttpPost("add-student-courses")]
        public async Task<IActionResult> AddStudentCourse([FromBody]  Student_Courses[] addStudentCourseRequest)
        {
            for (int i = 0; i < addStudentCourseRequest.Length ; i++)
            {
                await _authContext.Student_Courses.AddAsync(addStudentCourseRequest[i]);
                await _authContext.SaveChangesAsync();
            }


            return Ok(addStudentCourseRequest);
        }
    }
}
