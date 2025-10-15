using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace bookdb.Models
{
    public class Borrowing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên người mượn từ 3 đến 100 ký tự.")]
        public string ReaderName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string ReaderEmail { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        [Range(0.0, 1.0)]
        public decimal? ReturnStatus
        {
            get; set;

        }
    }
}
