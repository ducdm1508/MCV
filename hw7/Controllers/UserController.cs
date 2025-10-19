using hw7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace hw7.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private List<Users> ReadUser()
        {

            var json = System.IO.File.ReadAllText(Path.Combine("Data", "Users.json"));
            return JsonSerializer.Deserialize<List<Users>>(json) ?? new List<Users>();
        }

        [HttpGet("me")]
        public IActionResult GetMyInfo()
        {
            var users = ReadUser();
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng trong token." });

            var user = users.FirstOrDefault(u => u.GetType().GetProperty("Username")?.GetValue(u)?.ToString() == username);

            if (user == null)
                return NotFound(new { message = "Không tìm thấy người dùng." });

            return Ok(user);
        }
    }
}
