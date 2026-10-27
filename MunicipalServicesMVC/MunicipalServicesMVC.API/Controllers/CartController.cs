using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Data;

namespace MunicipalServicesMVC.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // عرض سلة المستخدم الحالي
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var items = await _context.CartItems
                .AsNoTracking()
                .Where(item => item.UserId == userId)
                .OrderBy(item => item.Id)
                .Select(item => new
                {
                    item.Id,
                    item.ServiceId,
                    item.Quantity,
                    Name = item.Service.Name,
                    Description = item.Service.Description,
                    DepartmentId = item.Service.DepartmentId
                })
                .ToListAsync();

            return Ok(items);
        }

        // إضافة خدمة إلى السلة أو زيادة كميتها
        [HttpPost]
        public async Task<IActionResult> AddToCart(
            [FromBody] AddCartDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var serviceExists = await _context.Services
                .AnyAsync(service => service.Id == dto.ServiceId);

            if (!serviceExists)
            {
                return NotFound(new
                {
                    message = "الخدمة غير موجودة."
                });
            }

            var item = await _context.CartItems
                .FirstOrDefaultAsync(item =>
                    item.UserId == userId &&
                    item.ServiceId == dto.ServiceId);

            if (item == null)
            {
                item = new CartItem
                {
                    UserId = userId,
                    ServiceId = dto.ServiceId,
                    Quantity = dto.Quantity
                };

                _context.CartItems.Add(item);
            }
            else
            {
                if (item.Quantity > int.MaxValue - dto.Quantity)
                {
                    return BadRequest(new
                    {
                        message = "الكمية المطلوبة كبيرة جدًا."
                    });
                }

                item.Quantity += dto.Quantity;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "تمت إضافة الخدمة إلى قائمة الطلب."
            });
        }

        // حذف خدمة من سلة المستخدم الحالي
        [HttpDelete("{serviceId:int}")]
        public async Task<IActionResult> RemoveFromCart(int serviceId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var item = await _context.CartItems
                .FirstOrDefaultAsync(item =>
                    item.UserId == userId &&
                    item.ServiceId == serviceId);

            if (item == null)
            {
                return NotFound(new
                {
                    message = "الخدمة غير موجودة في قائمة الطلب."
                });
            }

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "تم حذف الخدمة من قائمة الطلب."
            });
        }

        private bool TryGetUserId(out int userId)
        {
            return int.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out userId) && userId > 0;
        }
    }

    public class AddCartDto
    {
        [Range(1, int.MaxValue)]
        public int ServiceId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }
}