using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Registration_System_Backend.Context;
using Student_Registration_System_Backend.Models;

namespace Student_Registration_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        public CourseController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;
        }

        [HttpPost("add-course")]
        public async Task<IActionResult> AddCourse([FromBody] Course addCourseRequest)
        {
            await _authContext.Courses.AddAsync(addCourseRequest);
            await _authContext.SaveChangesAsync();

            return Ok(addCourseRequest);
        }
    }
}
