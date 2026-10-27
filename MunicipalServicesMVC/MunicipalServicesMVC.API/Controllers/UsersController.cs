using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Application.Dtos;
using MunicipalServicesMVC.Application.Services;
using MunicipalServicesMVC.Infrastructure.Data;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public UsersController(
            IUserService userService,
            AppDbContext db,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment environment)
        {
            _userService = userService;
            _db = db;
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        // إنشاء حساب
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(
            [FromBody] RegisterDto registerDto)
        {
            var success = _userService.Register(registerDto);

            if (!success)
            {
                return Conflict(new
                {
                    message = "البريد الإلكتروني مسجل مسبقًا"
                });
            }

            return Ok(new
            {
                message = "تم إنشاء الحساب بنجاح"
            });
        }

        // تسجيل الدخول
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto loginDto)
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            var success = _userService.Login(loginDto);

            if (!success)
            {
                return Unauthorized(new
                {
                    message = "البريد الإلكتروني أو كلمة المرور غير صحيحة"
                });
            }

            var email = loginDto.Email.Trim();

            var user = await _db.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "تعذر تسجيل الدخول"
                });
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.Name),

                new Claim(
                    ClaimTypes.Email,
                    user.Email)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = false
                });

            return Ok(new
            {
                message = "تم تسجيل الدخول بنجاح",
                user = new
                {
                    user.Id,
                    user.Name,
                    user.Email
                }
            });
        }

        // بيانات الملف الشخصي
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            if (!int.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId))
            {
                return Unauthorized();
            }

            var user = await _db.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.EmployeeId,
                    EmployeeName = u.Employee != null
                        ? u.Employee.Name
                        : null
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized();
            }

            var image = GetEmployeeImage(user.EmployeeId);

            return Ok(new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                employeeName = user.EmployeeName,
                imageUrl = image == null
                    ? null
                    : "/api/Users/me/photo"
            });
        }

        // صورة الملف الشخصي
        [Authorize]
        [HttpGet("me/photo")]
        public async Task<IActionResult> Photo()
        {
            if (!int.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId))
            {
                return Unauthorized();
            }

            var user = await _db.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.EmployeeId
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized();
            }

            var image = GetEmployeeImage(user.EmployeeId);

            if (image == null)
            {
                return NotFound();
            }

            Response.Headers["Cache-Control"] = "no-store";

            return PhysicalFile(
                image.Value.Path,
                image.Value.ContentType);
        }

        private (string Path, string ContentType)? GetEmployeeImage(
            int? employeeId)
        {
            if (!employeeId.HasValue)
            {
                return null;
            }

            var imageUrl = _unitOfWork.Employees
                .GetFiles(employeeId.Value)
                .OrderByDescending(f => f.Id)
                .Select(f => f.FileURL)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return null;
            }

            const string prefix = "/Files/Employees/";

            if (!imageUrl.StartsWith(
                prefix,
                StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var fileName = imageUrl.Substring(prefix.Length);

            if (string.IsNullOrWhiteSpace(fileName) ||
                fileName.Contains('/') ||
                fileName.Contains('\\') ||
                fileName == "." ||
                fileName == "..")
            {
                return null;
            }

            var contentType =
                Path.GetExtension(fileName).ToLowerInvariant() switch
                {
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".webp" => "image/webp",
                    _ => null
                };

            if (contentType == null)
            {
                return null;
            }

            var filePath = Path.GetFullPath(
                Path.Combine(
                    _environment.ContentRootPath,
                    "..",
                    "wwwroot",
                    "Files",
                    "Employees",
                    fileName));

            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }

            return (filePath, contentType);
        }

        // تسجيل الخروج
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(new
            {
                message = "تم تسجيل الخروج"
            });
        }
    }
}