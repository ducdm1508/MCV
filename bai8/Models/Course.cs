using System.ComponentModel.DataAnnotations;

namespace bai8.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string CourseName { get; set; }

        public int Credits { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
