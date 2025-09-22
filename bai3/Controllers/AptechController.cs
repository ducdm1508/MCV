using bai3.Models;
using Microsoft.AspNetCore.Mvc;

namespace bai3.Controllers
{
    public class AptechController : Controller
    {
        public static List<Student> students = new List<Student>()
        {
            new Student { id = 1, name = "Nguyễn Văn A", age = 20 },
            new Student { id = 2, name = "Trần Thị B", age = 21 },
            new Student { id = 3, name = "Lê Văn C", age = 22 }
        };
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Student()
        {
            return View(students);
        }
        [Route("/aptech/student/{id}")]
        public IActionResult StudentDetail(int id)
        {
            var student = students.FirstOrDefault(s => s.id == id);
            if (student == null) return NotFound("Không tìm thấy sinh viên!");
            return View(student);
        }
    }
}
