using codefirt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace codefirt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthorController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _context.Author
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
                return NotFound(new { message = "Không tìm thấy tác giả" });

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Author.Add(author);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new {id = author.Id}, author);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var author = await _context.Author.ToListAsync();
            return Ok(author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]Author author)
        {
            var au = await _context.Author.FirstOrDefaultAsync(a => a.Id == id);
            if (au == null)
            {
                return NotFound();
            }

            au.Name = author.Name;
            au.Biography = author.Biography;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var au = await _context.Author.FirstOrDefaultAsync(a => a.Id == id);
            if (au == null)
            {
                return NotFound();
            }

            _context.Author.Remove(au);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
