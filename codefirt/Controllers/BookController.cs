using codefirt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace codefirt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BookController (AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var books = await _context.Book
                .Include(b => b.author)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.PublicationYear,
                    AuthorName = b.author.Name,
                    AuthorId = b.AuthorId
                })
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _context.Book
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

 
            var existingAuthor = await _context.Author
                .FirstOrDefaultAsync(a => a.Id == book.AuthorId);

            if (existingAuthor == null)
            {
                return BadRequest(new { message = "AuthorId không tồn tại!" });
            }

            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Book book)
        {

            var bk = await _context.Book.FirstOrDefaultAsync(b => b.Id == id);
            if(bk == null)
            {
                return NotFound();
            }

            bk.Title = book.Title;
            bk.PublicationYear = book.PublicationYear;
            bk.AuthorId = book.AuthorId;


            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bk = await _context.Book.FirstOrDefaultAsync(b => b.Id == id);
            if (bk == null)
            {
                return NotFound();
            }

            _context.Book.Remove(bk);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}
