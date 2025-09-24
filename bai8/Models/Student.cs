using System.ComponentModel.DataAnnotations;

namespace bai8.Models
{

    public class Student
        {
            public int Id { get; set; }

            [Required]
            public string FullName { get; set; }

            public DateTime BirthDate { get; set; }

            public string ClassName { get; set; }

            
            public ICollection<Course> Courses { get; set; } = new List<Course>();
        }
}
