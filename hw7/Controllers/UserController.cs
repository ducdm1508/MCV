using hw7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace hw7.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string _connectionString => _configuration.GetConnectionString("DefaultConnection");

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("me")]
        public IActionResult GetMyInfo()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng trong token." });

            Users? user = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT Username, Email, Role FROM Users WHERE Username = @Username";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new Users
                            {
                                Username = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }
            }

            if (user == null)
                return NotFound(new { message = "Không tìm thấy người dùng." });

            return Ok(user);
        }

        [Authorize(Roles ="admin")]
        [HttpGet]
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetAllUser()
        {
            List<Users> userList = new List<Users>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT Username, Password, Email, Role, RefreshToken FROM Users";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new Users
                            {
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString(),
                                RefreshToken = reader["RefreshToken"] == DBNull.Value
                                    ? null
                                    : reader["RefreshToken"].ToString()
                            };
                            userList.Add(user);
                        }
                    }
                }
            }

            if (userList.Count == 0)
                return NotFound(new { message = "Không tìm thấy người dùng." });

            return Ok(userList);
        }

    }
}
