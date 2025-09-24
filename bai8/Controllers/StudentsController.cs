using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bai8.Data;
using bai8.Models;

namespace bai8.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var students = _context.Students
                                   .Include(s => s.Courses)
                                   .ToList();
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            ViewBag.Courses = _context.Courses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student, int[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                if (selectedCourses != null)
                {
                    student.Courses = _context.Courses
                                              .Where(c => selectedCourses.Contains(c.Id))
                                              .ToList();
                }

                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Courses = _context.Courses.ToList();
            return View(student);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _context.Students
                                  .Include(s => s.Courses)
                                  .FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();

            ViewBag.Courses = _context.Courses.ToList();
            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student, int[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                var studentToUpdate = _context.Students
                                              .Include(s => s.Courses)
                                              .FirstOrDefault(s => s.Id == student.Id);

                if (studentToUpdate == null) return NotFound();


                studentToUpdate.FullName = student.FullName;
                studentToUpdate.BirthDate = student.BirthDate;
                studentToUpdate.ClassName = student.ClassName;


                studentToUpdate.Courses.Clear();
                if (selectedCourses != null)
                {
                    studentToUpdate.Courses = _context.Courses
                                                      .Where(c => selectedCourses.Contains(c.Id))
                                                      .ToList();
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Courses = _context.Courses.ToList();
            return View(student);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _context.Students
                                  .Include(s => s.Courses)
                                  .FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _context.Students
                                  .Include(s => s.Courses)
                                  .FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
