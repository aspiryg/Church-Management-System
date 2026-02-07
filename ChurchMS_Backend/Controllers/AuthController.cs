using ChurchMS_Backend.Entities;
using ChurchMS_Backend.Models;
using ChurchMS_Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChurchMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _auth) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await _auth.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("User already exists.");
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var token = await _auth.LoginAsync(request);
            if (token == null) {
                return BadRequest("Invalid username or password.");
            }
            return Ok(token);
        }
    }
}
