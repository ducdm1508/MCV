using System.ComponentModel.DataAnnotations.Schema;

namespace studentmn.Models
{
    [Table("Students")]
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string ClassName { get; set; }

     
        public ICollection<StudentScore> StudentScore { get; set; }

    }

}
