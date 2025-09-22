using System.ComponentModel.DataAnnotations.Schema;

namespace studentmn.Models
{
    [Table("StudentScores")]
    public class StudentScore
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public decimal Score { get; set; }

        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
