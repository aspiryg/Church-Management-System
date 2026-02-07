using ChurchMS_Backend.Data;
using ChurchMS_Backend.Entities;
using ChurchMS_Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChurchMS_Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserDbContext _db;
        private readonly IConfiguration _configuration;

        public AuthService(UserDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public async Task<string?> LoginAsync(UserDto request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return null!;
            }
            if (new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null!;
            }
            string token = CreateToken(user);
            return token;
        }

        public async Task<User> RegisterAsync(UserDto request)
        {
            if( await _db.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null!;
            }
            var user = new User();
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);
            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }


        private string CreateToken(User user)
        {
            //
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwt;
        }
    }
}
