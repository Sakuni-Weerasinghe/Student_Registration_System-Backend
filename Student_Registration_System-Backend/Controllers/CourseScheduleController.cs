using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Registration_System_Backend.Context;
using Student_Registration_System_Backend.Models;
using Student_Registration_System_Backend.NewFolder;

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
        public async Task<IActionResult> AddCourseSchedule([FromBody] CourseScheduleRequestDto addCourseScheduleRequest)
        {
            // Step 1: Retrieve CourseId for the given CourseCode
            var course = _authContext.Courses.FirstOrDefault(c => c.CourseCode == addCourseScheduleRequest.CourseCode);

            if (course == null)
            {
                // Handle the case where the CourseCode is not found
                return NotFound( new { Message = $"Course with code {addCourseScheduleRequest.CourseCode} not found" });
            }

            // Step 2: Check if the schedule already exists for any course
            var existingSchedule = _authContext.CourseSchedules
                .FirstOrDefault(s => s.Date == addCourseScheduleRequest.Date &&
                                      s.Time == addCourseScheduleRequest.Time &&
                                      s.Venue == addCourseScheduleRequest.Venue);

            if (existingSchedule != null)
            {
                // Handle the case where the schedule already exists
                return Conflict(new
                {
                    Message = "Schedule already exists"
                });
            }

            // Step 4: Create a new instance of CourseSchedule
            var newSchedule = new CourseSchedule
            {
                CourseId = course.CourseId,
                Date = addCourseScheduleRequest.Date,
                Time = addCourseScheduleRequest.Time,
                Venue = addCourseScheduleRequest.Venue
            };

            // Step 5: Save the new CourseSchedule to the database
            _authContext.CourseSchedules.Add(newSchedule);
            _authContext.SaveChanges();

            // Optionally, you can return a response indicating success
            return Ok(new { Message = "Course schedule added successfully" });

        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCourseSchedule([FromRoute] int id)
        {
            var courseSchedule = await _authContext.CourseSchedules
                .Where(x => x.CourseId == id)
                .ToListAsync();

            if (courseSchedule == null)
            {
                return NotFound();
            }
            return Ok(courseSchedule);
        }

        //[HttpPut]
        //[Route("update/{courseScheduleId:int}")]
        //public async Task<IActionResult> UpdateCourseSchedule([FromRoute] int courseScheduleId, [FromBody] CourseSchedule updateCourseScheduleRequest)
        //{
        //    var courseSchedule = await _authContext.CourseSchedules.FindAsync(courseScheduleId);

        //    if (courseSchedule == null)
        //    {
        //        return NotFound(new { Message = "Course schedule not found" });
        //    }

        //    courseSchedule.Date = updateCourseScheduleRequest.Date;
        //    courseSchedule.Time = updateCourseScheduleRequest.Time;
        //    courseSchedule.Venue = updateCourseScheduleRequest.Venue;

        //    await _authContext.SaveChangesAsync();

        //    return Ok(new { Message = "Course schedule successfully updated" });
        //}

        [HttpDelete]
        [Route("delete/{courseScheduleId:int}")]
        public async Task<IActionResult> DeleteCourseSchedule([FromRoute] int courseScheduleId)
        {
            var course_schedule = await _authContext.CourseSchedules.FindAsync(courseScheduleId);
            if (course_schedule == null)
            {
                return NotFound();
            }
            _authContext.CourseSchedules.Remove(course_schedule);
            await _authContext.SaveChangesAsync();
            return Ok(new { Message = "Successfully deleted!" });

        }
    }
}
