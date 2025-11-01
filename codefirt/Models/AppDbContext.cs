using Microsoft.EntityFrameworkCore;

namespace codefirt.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Book {  get; set; }
        public DbSet<Author> Author { get; set; }
    }
}
