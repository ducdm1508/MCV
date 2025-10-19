using hw7.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace hw7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController (IConfiguration config)
        {
            _config = config;
        }

        private List<Users> ReadUser()
        {
            var json = System.IO.File.ReadAllText(Path.Combine("Data", "Users.json"));
            return JsonSerializer.Deserialize<List<Users>>(json) ?? new List<Users>();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            var users = ReadUser();
            var user = users.FirstOrDefault(u =>
                u.Username.Equals(login.Username, StringComparison.OrdinalIgnoreCase)
                && u.Password == login.Password);

            if (user == null)
            {
                return NotFound(new {message = "Sai tài khoản pass"} );
            }
            var token = GenerateToken(user);
            return Ok(
                new
                {
                    accessToken = token
                });
           
        }

        private string GenerateToken(Users user)
        {
            var Jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: Jwt["Issuer"],
                audience: Jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
