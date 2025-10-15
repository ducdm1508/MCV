using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hw6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretController : ControllerBase
    {
        [Authorize]
        [HttpGet("data")]
        public IActionResult GetSecretData()
        {
            var username = User.Identity.Name;
            return Ok($"Xin chào {username}! Đây là dữ liệu bí mật.");
        }
    }
}
