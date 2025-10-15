using System.ComponentModel.DataAnnotations;

namespace bookdb.Models
{
    public class Book
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Title phải từ 5 đến 255 ký tự.")]
        public string Title { get; set; }

        [Range(1, 2000, ErrorMessage = "Pages phải từ 1 đến 2000.")]
        public int Pages { get; set; }

        public string Genre { get; set; }
    }
}
