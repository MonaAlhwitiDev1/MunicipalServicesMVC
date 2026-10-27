using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // Login - GET
        // =========================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // =========================
        // Login - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string email,
            string password)
        {
            var user =
                _unitOfWork.Users
                           .GetByEmailWithRoleAndEmployee(email);

            if (user == null)
            {
                ViewBag.Error =
                    "البريد الإلكتروني أو كلمة المرور غير صحيحة";

                return View();
            }

            var passwordHasher =
                new PasswordHasher<User>();

            var result =
                passwordHasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    password
                );

            if (result ==
                PasswordVerificationResult.Failed)
            {
                ViewBag.Error =
                    "البريد الإلكتروني أو كلمة المرور غير صحيحة";

                return View();
            }

            // =========================
            // الصورة المرتبطة بالموظف
            // =========================
            string? employeeImage = null;

            if (user.EmployeeId.HasValue)
            {
                employeeImage =
                    _unitOfWork.Employees
                               .GetFiles(user.EmployeeId.Value)
                               .OrderByDescending(f => f.Id)
                               .Select(f => f.FileURL)
                               .FirstOrDefault();
            }

            // =========================
            // Claims
            // =========================
            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.Name
                ),

                new Claim(
                    ClaimTypes.Email,
                    user.Email
                ),

                new Claim(
                    "RoleId",
                    user.RoleId.ToString()
                )
            };

            // =========================
            // بيانات الموظف
            // =========================
            if (user.Employee != null)
            {
                claims.Add(
                    new Claim(
                        "EmployeeName",
                        user.Employee.Name
                    )
                );
            }

            if (!string.IsNullOrEmpty(employeeImage))
            {
                claims.Add(
                    new Claim(
                        "EmployeeImage",
                        employeeImage
                    )
                );
            }

            // =========================
            // Role + Permissions
            // =========================
            if (user.Role != null)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        user.Role.Name
                    )
                );

                var rolePermissions =
                    _unitOfWork.Roles
                               .GetRolePermissions(user.RoleId);

                foreach (var rolePermission in rolePermissions)
                {
                    var permission =
                        _unitOfWork.Permissions
                                   .GetById(
                                       rolePermission.PermissionId
                                   );

                    if (permission != null)
                    {
                        claims.Add(
                            new Claim(
                                "Permission",
                                permission.Name
                            )
                        );
                    }
                }
            }

            // =========================
            // إنشاء تسجيل الدخول
            // =========================
            var identity =
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults
                        .AuthenticationScheme
                );

            var principal =
                new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme,
                principal
            );

            return RedirectToAction(
                "Index",
                "Home"
            );
        }

        // =========================
        // Logout
        // =========================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme
            );

            return RedirectToAction(
                "Login",
                "Account"
            );
        }

        // =========================
        // Access Denied
        // =========================
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}