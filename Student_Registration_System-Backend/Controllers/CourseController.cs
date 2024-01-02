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
            HelpingMethods.TrimStringProperties(addCourseRequest);
            if (_authContext.Courses.Any(s => s.CourseCode == addCourseRequest.CourseCode))
            {
                // Student with the same registration number already exists
                return BadRequest(new { Message = "The course is already added!" });
            }
            addCourseRequest.RegisterDate = DateTime.UtcNow.Date ;
            await _authContext.Courses.AddAsync(addCourseRequest);
            await _authContext.SaveChangesAsync();

            return Ok(new {Message = "Course Registered" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courseList = await _authContext.Courses
                .Include(x => x.CourseSchedules).ToListAsync();

            return Ok(courseList);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCourse([FromRoute] int id)
        {
            var course = await _authContext.Courses
                .Include(x => x.CourseSchedules)
                .FirstOrDefaultAsync(x => x.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPut]
        [Route("update/{courseId:int}")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int courseId, Course updateCourseRequest)
        {
            var course = await _authContext.Courses
                .Include(c => c.CourseSchedules)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null)
            {
                return NotFound(new { Message = "Not Found" });
            }

            course.CourseName = updateCourseRequest.CourseName;
            course.CourseCode = updateCourseRequest.CourseCode;
            course.Credits = updateCourseRequest.Credits;
            course.Lecturer = updateCourseRequest.Lecturer;

            // Update associated course schedules
            foreach (var updatedSchedule in updateCourseRequest.CourseSchedules)
            {
                var schedule = course.CourseSchedules.FirstOrDefault(s => s.CourseScheduleId == updatedSchedule.CourseScheduleId);

                if (schedule != null)
                {
                // Update existing schedule
                schedule.Date = updatedSchedule.Date;
                schedule.Time = updatedSchedule.Time;
                schedule.Venue = updatedSchedule.Venue;

                await _authContext.SaveChangesAsync();
                }
            }

            await _authContext.SaveChangesAsync();

            //return Ok(new
            //{
            //    Message = "Successfully Updated!"
            //});
            return Ok(course);
        }



        [HttpDelete]
        [Route("delete/{courseId:int}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int courseId)
        {
            var course = await _authContext.Courses
                .Include(s => s.StudentCourses)  // Include related courses to delete them as well
                .Include(s => s.CourseSchedules)
                .FirstOrDefaultAsync(s => s.CourseId == courseId);

            if (course == null)
            {
                return NotFound();
            }

            if (course.StudentCourses.Any())
            {
                // Remove related students
                _authContext.StudentCourses.RemoveRange(course.StudentCourses);
            }

            if (course.CourseSchedules.Any())
            {
                // Remove related courses schedule
                _authContext.CourseSchedules.RemoveRange(course.CourseSchedules);
            }


            // Remove the student
            _authContext.Courses.Remove(course);

            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Successfully Deleted" });

        }

    }
}
