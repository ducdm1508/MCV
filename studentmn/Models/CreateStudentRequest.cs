namespace studentmn.Models
{
    public class CreateStudentRequest
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string ClassName { get; set; }

        public Dictionary<int, decimal> SubjectScores { get; set; } = new Dictionary<int, decimal>();

        public List<Subject> AllSubjects { get; set; } = new List<Subject>();
    }
}