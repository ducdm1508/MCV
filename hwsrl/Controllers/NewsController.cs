using hwsrl.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace hwsrl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {

        private static List<News> _newsList = new List<News>
        {
            new News { Id = 1, Title = "Tin đầu tiên", Content = "Nội dung bài viết 1", Author = "Admin" },
            new News { Id = 2, Title = "Tin thứ hai", Content = "Nội dung bài viết 2", Author = "Staff" }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            Log.Information("[GET] Lấy toàn bộ danh sách tin tức");
            return Ok(_newsList);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var news = _newsList.FirstOrDefault(n => n.Id == id);
            if (news == null)
            {
                Log.Warning("[GET] Không tìm thấy bài viết có ID = {Id}", id);
                return NotFound(new { message = "Không tìm thấy bài viết." });
            }

            Log.Information("[GET] Lấy bài viết có ID = {Id}", id);
            return Ok(news);
        }

        [HttpPost]
        public IActionResult Create([FromBody] News newNews)
        {
            if (newNews == null)
            {
                Log.Warning("[POST] Dữ liệu bài viết không hợp lệ");
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            newNews.Id = _newsList.Any() ? _newsList.Max(n => n.Id) + 1 : 1;
            newNews.CreatedAt = DateTime.Now;
            _newsList.Add(newNews);

            Log.Information("[POST] Tạo bài viết mới: {@News}", newNews);
            return CreatedAtAction(nameof(GetById), new { id = newNews.Id }, newNews);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] News updatedNews)
        {
            var existingNews = _newsList.FirstOrDefault(n => n.Id == id);
            if (existingNews == null)
            {
                Log.Warning("[PUT] Không tìm thấy bài viết có ID = {Id}", id);
                return NotFound(new { message = "Không tìm thấy bài viết." });
            }

            existingNews.Title = updatedNews.Title;
            existingNews.Content = updatedNews.Content;
            existingNews.Author = updatedNews.Author;

            Log.Information("[PUT] Cập nhật bài viết ID = {Id} thành công: {@News}", id, existingNews);
            return Ok(existingNews);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var news = _newsList.FirstOrDefault(n => n.Id == id);
            if (news == null)
            {
                Log.Warning("[DELETE] Không tìm thấy bài viết có ID = {Id}", id);
                return NotFound(new { message = "Không tìm thấy bài viết." });
            }

            _newsList.Remove(news);
            Log.Information("[DELETE] Đã xóa bài viết ID = {Id}", id);

            return NoContent();
        }
    }
}
