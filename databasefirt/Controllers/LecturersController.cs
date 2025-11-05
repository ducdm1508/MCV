using databasefirt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace databasefirt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturersController : ControllerBase
    {

        private readonly UniversityDbContext _context;
        public LecturersController(UniversityDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10, string? search = null) 
        {
            var lecturers = _context.Lecturers
                .Include(l => l.Department)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search)) 
            {
                lecturers = lecturers.Where(lec => lec.FullName.Contains(search) || lec.Degree.Contains(search) || lec.Department.DepartmentName.Contains(search));
            }

            var totalRecord = await lecturers.CountAsync();
            var totalPage = (int)Math.Ceiling( (double) totalRecord/ pageSize);

            var lecturerList = await lecturers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(l=> new
                {
                    l.LecturerId,
                    l.FullName,
                    l.Degree,
                    l.DepartmentId,
                    DepartmentName = l.Department.DepartmentName

                }    
                )
                .ToListAsync();

            return Ok(new
            {
                TotalRecord = totalRecord,
                TotalPage = totalPage,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Search = search,
                Data = lecturerList
            });
        }
    }
}
