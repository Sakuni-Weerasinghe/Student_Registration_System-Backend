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
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        public AdminController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Admin adminObj)
        {
            if (adminObj == null)
                return BadRequest();

            var admin = await _authContext.Admins
                .FirstOrDefaultAsync(x => x.UserName == adminObj.UserName);

            if (admin == null)
            {
                return NotFound(new { Message = "User Not Found!" });
            }
            if (!PasswordHasher.VerifyPassword(adminObj.Password, admin.Password))
            {
                return BadRequest(new { Message = "Password in incorrect" });
            }

            return Ok(new
            {
                Message = "Login Successs!"
            });
        }



        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Admin adminObj)
        {
            if (adminObj == null)
            {
                return BadRequest();
            }
            adminObj.Password = PasswordHasher.HashPassword(adminObj.Password);
            adminObj.Role = "Admin";
            adminObj.Token = "";
            await _authContext.Admins.AddAsync(adminObj); //help to send user to db
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Admin Registered!" });
        }

    }
}
