using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Registration_System_Backend.Context;
using Student_Registration_System_Backend.Models;

namespace Student_Registration_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseScheduleController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        public CourseScheduleController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;
        }

        [HttpPost("add-course-schedule")]
        public async Task<IActionResult> AddCourseSchedule([FromBody] CourseSchedule addCourseScheduleRequest)
        {
            await _authContext.CourseSchedules.AddAsync(addCourseScheduleRequest);
            await _authContext.SaveChangesAsync();

            return Ok(addCourseScheduleRequest);
        }
    }
}
