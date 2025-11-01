using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace codefirt.Models
{
    public class Book
    {

        public int Id { get; set; }
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        [Range(1000, 2100, ErrorMessage = "Năm xuất bản không hợp lệ")]
        public int PublicationYear { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        public Author? author { get; set; }

    }
}
