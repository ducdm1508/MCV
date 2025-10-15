using bookdb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bookdb.Controllers
{
    public class BookController : Controller
    {
        public readonly AppDBContext _context;

        public BookController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index(string searchString, int page = 1)
        {
            int pageSize = 5;
            var books = _context.Books.AsQueryable();


            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Genre.Contains(searchString));
            }

            int totalItems = books.Count();

            var bookOnPage = books.
                OrderBy(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.SearchString = searchString;


            return View(bookOnPage);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]

        public IActionResult Edit(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            return View(book);
        }
        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                var bookIp = _context.Books.FirstOrDefault(b => b.Id == book.Id);
                bookIp.Title = book.Title;
                bookIp.Pages = book.Pages;
                bookIp.Genre = book.Genre;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var bookIp = _context.Books.FirstOrDefault(b => b.Id == id);
            return View(bookIp);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Confirm(int id)
        {
            var bookIp = _context.Books.FirstOrDefault(b => b.Id == id);
            _context.Books.Remove(bookIp);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
