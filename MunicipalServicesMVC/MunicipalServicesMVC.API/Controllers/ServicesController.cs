using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Data;

namespace MunicipalServicesMVC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            var services = await _context.Services
                .AsNoTracking()
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Description,
                    s.DepartmentId
                })
                .ToListAsync();

            return Ok(services);
        }

        // GET: api/Services/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Services
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Description,
                    s.DepartmentId
                })
                .FirstOrDefaultAsync();

            if (service == null)
                return NotFound("الخدمة غير موجودة.");

            return Ok(service);
        }

        // POST: api/Services
        [HttpPost]
        public async Task<IActionResult> CreateService(
            [FromBody] ServiceRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("اسم الخدمة مطلوب.");

            if (request.DepartmentId.HasValue)
            {
                bool departmentExists = await _context.Departments
                    .AnyAsync(d => d.Id == request.DepartmentId.Value);

                if (!departmentExists)
                    return BadRequest("الإدارة المحددة غير موجودة.");
            }

            var service = new Service
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim() ?? string.Empty,
                DepartmentId = request.DepartmentId
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                service.Id,
                service.Name,
                service.Description,
                service.DepartmentId
            });
        }

        // PUT: api/Services/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(
            int id,
            [FromBody] ServiceRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("اسم الخدمة مطلوب.");

            var service = await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound("الخدمة غير موجودة.");

            if (request.DepartmentId.HasValue)
            {
                bool departmentExists = await _context.Departments
                    .AnyAsync(d => d.Id == request.DepartmentId.Value);

                if (!departmentExists)
                    return BadRequest("الإدارة المحددة غير موجودة.");
            }

            service.Name = request.Name.Trim();
            service.Description = request.Description?.Trim() ?? string.Empty;
            service.DepartmentId = request.DepartmentId;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                service.Id,
                service.Name,
                service.Description,
                service.DepartmentId
            });
        }

        // DELETE: api/Services/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound("الخدمة غير موجودة.");

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class ServiceRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? DepartmentId { get; set; }
    }
}