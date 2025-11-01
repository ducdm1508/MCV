using System.ComponentModel.DataAnnotations;

namespace codefirt.Models
{
    public class Author
    {

        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; }
    }
}
