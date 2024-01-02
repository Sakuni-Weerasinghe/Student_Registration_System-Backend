using Student_Registration_System_Backend.Context;
using Student_Registration_System_Backend.Models;

namespace Student_Registration_System_Backend.NewFolder1
{
    public class CourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateCourseScheduleAsync(int courseScheduleId, CourseSchedule updateCourseScheduleRequest)
        {
            var courseSchedule = await _context.CourseSchedules.FindAsync(courseScheduleId);

            if (courseSchedule != null)
            {
                courseSchedule.Date = updateCourseScheduleRequest.Date;
                courseSchedule.Time = updateCourseScheduleRequest.Time;
                courseSchedule.Venue = updateCourseScheduleRequest.Venue;

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
