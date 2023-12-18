﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Registration_System_Backend.Context;
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

    }
}