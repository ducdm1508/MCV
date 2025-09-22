using Microsoft.EntityFrameworkCore;
using studentmn.Models;

namespace studentmn.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options){ } 
        public DbSet<studentmn.Models.StudentScore> StudentScores { get; set; } = default!;
        public DbSet<studentmn.Models.Subject> Subjects { get; set; } = default!;

        public DbSet<studentmn.Models.Student> Students { get; set; } = default!;
    }
}
