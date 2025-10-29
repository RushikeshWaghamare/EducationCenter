using EducationCenter.Data.Models;
using EducationCenter.Dto.DTOs.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EducationCenter.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EducationCenterContext _context;
        private readonly IConfiguration _config;

        public AuthController(EducationCenterContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserRegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { message = "Email already registered." });

            var role = await _context.Roles.FindAsync(dto.RoleId);
            if (role == null) return BadRequest(new { message = "Invalid role." });

            CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = $"{Convert.ToBase64String(hash)}|{Convert.ToBase64String(salt)}",
                RoleId = dto.RoleId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User created." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _context.Users.Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return Unauthorized(new { message = "Invalid credentials" });

            var parts = user.PasswordHash.Split('|');
            if (parts.Length != 2) return Unauthorized(new { message = "Invalid credentials" });

            var storedHash = Convert.FromBase64String(parts[0]);
            var storedSalt = Convert.FromBase64String(parts[1]);

            if (!VerifyPasswordHash(dto.Password, storedHash, storedSalt))
                return Unauthorized(new { message = "Invalid credentials" });

            var token = CreateToken(user);
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    Role = user.Role?.RoleName
                }
            });
        }

        // ---------- Helpers ----------
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName)
        };

            // add role claim using RoleName if available
            if (user.Role != null)
                claims.Add(new Claim(ClaimTypes.Role, user.Role.RoleName));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(storedHash);
        }
    }
}
