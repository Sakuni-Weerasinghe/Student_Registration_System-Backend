﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Registration_System_Backend.Context;
using Student_Registration_System_Backend.Helpers;
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
        public async Task<IActionResult> AddStudentCourse([FromBody]  StudentCourse[] addStudentCourseRequest)
        {
            for (int i = 0; i < addStudentCourseRequest.Length ; i++)
            {
                HelpingMethods.TrimStringProperties(addStudentCourseRequest);
                await _authContext.StudentCourses.AddAsync(addStudentCourseRequest[i]);
                await _authContext.SaveChangesAsync();
            }
            return Ok(addStudentCourseRequest);
        }

        [HttpGet]
        [Route("Courses/{id:int}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            var studentCourses = await _authContext.StudentCourses.Where(x => x.StudentId == id).ToListAsync();
            if (studentCourses == null)
            {
                return NotFound();
            }
            return Ok(studentCourses) ;
        }
    }
}
