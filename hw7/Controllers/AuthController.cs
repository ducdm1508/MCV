using hw7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace hw7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string _connectionString => _configuration.GetConnectionString("DefaultConnection");

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
                return BadRequest(new { message = "Vui lòng nhập tài khoản và mật khẩu." });

            Users? users = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT Username, Role FROM Users WHERE Username = @username AND Password = @password";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@username", login.Username);
                    command.Parameters.AddWithValue("@password", login.Password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            users = new Users
                            {
                                Username = reader["Username"].ToString(),
                                Role = reader["Role"]?.ToString() ?? "user"
                            };
                        }
                    }
                }
            }

            if (users == null)
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu." });

            var token = GenerateJwtToken(users);
            var refreshToken = Guid.NewGuid().ToString();

            UpdateRefreshTokenInDb(users.Username, refreshToken);
            SetRefreshTokenCookie(refreshToken);

            return Ok(new { accessToken = token });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new { message = "Missing refresh token." });

            Users? users = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT Username, Role FROM Users WHERE RefreshToken = @RefreshToken";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@RefreshToken", refreshToken);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            users = new Users
                            {
                                Username = reader["Username"].ToString(),
                                Role = reader["Role"]?.ToString() ?? "user"
                            };
                        }
                    }
                }
            }

            if (users == null || string.IsNullOrEmpty(users.Username))
                return Unauthorized(new { message = "Invalid refresh token." });

            var newRefreshToken = Guid.NewGuid().ToString();
            UpdateRefreshTokenInDb(users.Username, newRefreshToken);
            SetRefreshTokenCookie(newRefreshToken);

            var newAccessToken = GenerateJwtToken(users);
            return Ok(new { accessToken = newAccessToken });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
              
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Users SET RefreshToken = NULL WHERE RefreshToken = @RefreshToken";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@RefreshToken", refreshToken);
                        command.ExecuteNonQuery();
                    }
                }
            }

            //  Xóa Cookie
            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1),
                Path = "/"
            });

            return Ok("Logged out successfully.");
        }

        private void UpdateRefreshTokenInDb(string username, string refreshToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "UPDATE Users SET RefreshToken = @RefreshToken WHERE Username = @Username";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@RefreshToken", (object)refreshToken ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/"
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        private string GenerateJwtToken(Users users)
        {
            var JwtSettings = _configuration.GetSection("Jwt");
            var secretKey = JwtSettings["SecretKey"];
            var issuer = JwtSettings["Issuer"];
            var audience = JwtSettings["Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, users.Username),
                new Claim(ClaimTypes.Role, users.Role ?? "user"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(30),
                signingCredentials: creds
    );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
