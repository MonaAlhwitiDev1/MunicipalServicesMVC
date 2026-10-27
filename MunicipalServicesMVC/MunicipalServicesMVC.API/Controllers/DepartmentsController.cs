using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Data;

namespace MunicipalServicesMVC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments
                .AsNoTracking()
                .Select(d => new
                {
                    d.Id,
                    d.UID,
                    d.Name
                })
                .ToListAsync();

            return Ok(departments);
        }

        // GET: api/Departments/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await _context.Departments
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Select(d => new
                {
                    d.Id,
                    d.UID,
                    d.Name
                })
                .FirstOrDefaultAsync();

            if (department == null)
                return NotFound("الإدارة غير موجودة.");

            return Ok(department);
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<IActionResult> CreateDepartment(
            [FromBody] DepartmentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("اسم الإدارة مطلوب.");

            var department = new Department
            {
                Name = request.Name.Trim()
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                department.Id,
                department.UID,
                department.Name
            });
        }

        // PUT: api/Departments/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(
            int id,
            [FromBody] DepartmentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("اسم الإدارة مطلوب.");

            var department = await _context.Departments
                .FindAsync(id);

            if (department == null)
                return NotFound("الإدارة غير موجودة.");

            department.Name = request.Name.Trim();
            await _context.SaveChangesAsync();

            return Ok(new
            {
                department.Id,
                department.UID,
                department.Name
            });
        }

        // DELETE: api/Departments/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments
                .FindAsync(id);

            if (department == null)
                return NotFound("الإدارة غير موجودة.");

            bool hasEmployees = await _context.Employees
                .AnyAsync(e => e.DepartmentId == id);

            bool hasServices = await _context.Services
                .AnyAsync(s => s.DepartmentId == id);

            if (hasEmployees || hasServices)
            {
                return BadRequest(
                    "لا يمكن حذف إدارة مرتبطة بموظفين أو خدمات.");
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class DepartmentRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}