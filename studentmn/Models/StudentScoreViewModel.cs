namespace studentmn.Models
{
    public class StudentScoreViewModel
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string ClassName { get; set; }
        public DateTime BirthDate { get; set; }
        public Dictionary<string, decimal?> SubjectScores { get; set; }
        public decimal AverageScore { get; set; }
    }
}
