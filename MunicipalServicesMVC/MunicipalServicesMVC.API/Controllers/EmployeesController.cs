using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Infrastructure.Data;
using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        // عرض جميع الموظفين
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.JobTitle,
                    e.DepartmentId
                })
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/Employees/10
        // عرض موظف واحد حسب الرقم
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.JobTitle,
                    e.DepartmentId
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound("الموظف غير موجود.");
            }

            return Ok(employee);
        }

        // POST: api/Employees
        // إضافة موظف جديد
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(
            [FromBody] CreateEmployeeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.JobTitle))
            {
                return BadRequest(
                    "اسم الموظف والمسمى الوظيفي مطلوبان.");
            }

            if (request.DepartmentId.HasValue)
            {
                bool departmentExists = await _context.Departments
                    .AnyAsync(d =>
                        d.Id == request.DepartmentId.Value);

                if (!departmentExists)
                {
                    return BadRequest(
                        "الإدارة المحددة غير موجودة.");
                }
            }

            var employee = new Employee
            {
                Name = request.Name.Trim(),
                JobTitle = request.JobTitle.Trim(),
                DepartmentId = request.DepartmentId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                employee.Id,
                employee.Name,
                employee.JobTitle,
                employee.DepartmentId
            });
        }

        // PUT: api/Employees/10
        // تعديل بيانات موظف
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(
            int id,
            [FromBody] UpdateEmployeeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.JobTitle))
            {
                return BadRequest(
                    "اسم الموظف والمسمى الوظيفي مطلوبان.");
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound("الموظف غير موجود.");
            }

            if (request.DepartmentId.HasValue)
            {
                bool departmentExists = await _context.Departments
                    .AnyAsync(d =>
                        d.Id == request.DepartmentId.Value);

                if (!departmentExists)
                {
                    return BadRequest(
                        "الإدارة المحددة غير موجودة.");
                }
            }

            employee.Name = request.Name.Trim();
            employee.JobTitle = request.JobTitle.Trim();
            employee.DepartmentId = request.DepartmentId;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                employee.Id,
                employee.Name,
                employee.JobTitle,
                employee.DepartmentId
            });
        }

        // DELETE: api/Employees/10
        // حذف موظف حسب الرقم
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees
                .FindAsync(id);

            if (employee == null)
            {
                return NotFound("الموظف غير موجود.");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // بيانات إضافة موظف
    public class CreateEmployeeRequest
    {
        public string Name { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public int? DepartmentId { get; set; }
    }

    // بيانات تعديل موظف
    public class UpdateEmployeeRequest
    {
        public string Name { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public int? DepartmentId { get; set; }
    }
}