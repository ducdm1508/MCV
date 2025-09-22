using bai6.Models;
using Microsoft.AspNetCore.Mvc;
using StudentValidationLib;

namespace bai6.Controllers
{
    public class StudentController : Controller
    {
        private static List<Student> _students = new List<Student>();

        private static List<Student> GenerateStudents(int count)
        {
            var list = new List<Student>();
            var rand = new Random();

            string[] firstNames = { "An", "Binh", "Chi", "Dung", "Hanh", "Lan", "Mai", "Son" };
            string[] middleNames = { "Van", "Thi", "Ngoc", "Thanh", "Hoang" };
            string[] lastNames = { "Nguyen", "Tran", "Le", "Pham", "Hoang", "Dang" };

            for (int i = 1; i <= count; i++)
            {
                list.Add(new Student
                {
                    Id = i,
                    FirstName = firstNames[rand.Next(firstNames.Length)],
                    MiddleName = middleNames[rand.Next(middleNames.Length)],
                    LastName = lastNames[rand.Next(lastNames.Length)],
                    Birthday = DateTime.Now.AddYears(-rand.Next(18, 25)).AddDays(rand.Next(365)),
                    EClass = "C" + rand.Next(1, 50),
                    Phone = "09" + rand.Next(10000000, 99999999),
                    Email = $"student{i}@gmail.com"
                });
            }
            return list;
        }

        static StudentController()
        {
            _students = GenerateStudents(100);
        }

        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            var totalStudents = _students.Count;
            var totalPages = (int)Math.Ceiling(totalStudents / (double)pageSize);

            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var students = _students
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(students);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Student());
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
    
            ModelState.Clear();

            if (string.IsNullOrWhiteSpace(student.FirstName))
                ModelState.AddModelError(nameof(student.FirstName), "First Name is required");

            if (string.IsNullOrWhiteSpace(student.LastName))
                ModelState.AddModelError(nameof(student.LastName), "Last Name is required");

            if (string.IsNullOrWhiteSpace(student.Email))
                ModelState.AddModelError(nameof(student.Email), "Email is required");

            if (string.IsNullOrWhiteSpace(student.Phone))
                ModelState.AddModelError(nameof(student.Phone), "Phone is required");

      
            string error;
            if (!string.IsNullOrWhiteSpace(student.FirstName) &&
                !StudentValidator.ValidateFirstName(student.FirstName, out error))
                ModelState.AddModelError(nameof(student.FirstName), error);

            if (!string.IsNullOrWhiteSpace(student.LastName) &&
                !StudentValidator.ValidateLastName(student.LastName, out error))
                ModelState.AddModelError(nameof(student.LastName), error);

            if (!string.IsNullOrWhiteSpace(student.Email) &&
                !StudentValidator.ValidateEmail(student.Email, out error))
                ModelState.AddModelError(nameof(student.Email), error);

            if (!string.IsNullOrWhiteSpace(student.Phone) &&
                !StudentValidator.ValidatePhone(student.Phone, out error))
                ModelState.AddModelError(nameof(student.Phone), error);

            if (!string.IsNullOrWhiteSpace(student.Email) &&
                _students.Any(s => s.Email.Equals(student.Email, StringComparison.OrdinalIgnoreCase)))
                ModelState.AddModelError(nameof(student.Email), "Email already exists");

            if (!string.IsNullOrWhiteSpace(student.Phone) &&
                _students.Any(s => s.Phone == student.Phone))
                ModelState.AddModelError(nameof(student.Phone), "Phone number already exists");

     
            foreach (var modelError in ModelState)
            {
                if (modelError.Value.Errors.Count > 0)
                {
                    Console.WriteLine($"Field: {modelError.Key}, Errors: {string.Join(", ", modelError.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is not valid, returning view");
                return View(student);
            }
            student.Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;

  
            if (student.Birthday == default(DateTime))
                student.Birthday = DateTime.Now.AddYears(-20);

            if (string.IsNullOrWhiteSpace(student.EClass))
                student.EClass = "C1"; 

            _students.Add(student);

            Console.WriteLine($"Student added successfully. Total students: {_students.Count}");

            return RedirectToAction("Index");
        }
    }
}
