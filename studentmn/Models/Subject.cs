using System.ComponentModel.DataAnnotations.Schema;

namespace studentmn.Models
{
    [Table("Subjects")]
    public class Subject
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
    }
}
