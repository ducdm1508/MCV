using databasefirt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace databasefirt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly UniversityDbContext _context;

        public DepartmentsController(UniversityDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dp = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            return Ok(dp);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNum = 1, int pageSize = 10, string? search = null ) { 
            var dps = _context.Departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                dps = dps.Where(d => d.DepartmentName.Contains(search) || d.Dean.Contains(search));
            }

            var total = await dps.CountAsync();
            var totalPage = (int)Math.Ceiling((double)total / pageSize);

            var departments = await dps
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return Ok(new
            {
                Total = total,
                PageNum = pageNum,
                TotalPage = totalPage ,
                PageSize = pageSize,
                Data = departments
            }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Department department)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = department.DepartmentId }, department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Department department)
        {
            var dpm = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if(dpm == null)
            {
                return NotFound();
            }

            dpm.DepartmentName = department.DepartmentName;
            dpm.Dean = department.Dean;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dpm = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
            if (dpm == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(dpm);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}
