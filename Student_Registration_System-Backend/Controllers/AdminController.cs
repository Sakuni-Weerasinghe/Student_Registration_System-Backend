using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Student_Registration_System_Backend.Context;
using Student_Registration_System_Backend.Helpers;
using Student_Registration_System_Backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                return NotFound(new { Message = "User Name is incorrect!" });
            }
            if (!PasswordHasher.VerifyPassword(adminObj.Password, admin.Password))
            {
                return BadRequest(new { Message = "Password in incorrect!" });
            }

            admin.Token = CreateJwt(admin);
            return Ok(new
            {
                Token = admin.Token,
                Message = "Login Successs!"
            }) ;
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
            HelpingMethods.TrimStringProperties(adminObj);
            await _authContext.Admins.AddAsync(adminObj); //help to send user to db
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Admin Registered!" });
        }

        private string CreateJwt(Admin admin) 
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authentication");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role,admin.Role)
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(120),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        [HttpGet]
        public async Task<ActionResult<Admin>> GetAllAdmins()
        {
            return Ok(await _authContext.Admins.ToListAsync());
        }

    }
}
