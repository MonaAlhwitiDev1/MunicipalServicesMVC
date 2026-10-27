using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize(Roles = "مدير النظام")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // عرض المستخدمين
        // =========================
        public IActionResult Index()
        {
            var users =
                _unitOfWork.Users.GetAllWithRoleAndEmployee();

            return View(users);
        }

        // =========================
        // Create - GET
        // =========================
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(
                _unitOfWork.Roles.GetAll(),
                "Id",
                "Name"
            );

            ViewBag.Employees = new SelectList(
                _unitOfWork.Employees
                           .GetAllWithDepartment()
                           .OrderBy(e => e.Name),
                "Id",
                "Name"
            );

            return View();
        }

        // =========================
        // Create - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var passwordHasher =
                    new PasswordHasher<User>();

                user.Password =
                    passwordHasher.HashPassword(
                        user,
                        user.Password
                    );

                _unitOfWork.Users.Add(user);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(
                _unitOfWork.Roles.GetAll(),
                "Id",
                "Name",
                user.RoleId
            );

            ViewBag.Employees = new SelectList(
                _unitOfWork.Employees
                           .GetAllWithDepartment()
                           .OrderBy(e => e.Name),
                "Id",
                "Name",
                user.EmployeeId
            );

            return View(user);
        }

        // =========================
        // Edit - GET
        // =========================
        public IActionResult Edit(int id)
        {
            var user =
                _unitOfWork.Users.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = new SelectList(
                _unitOfWork.Roles.GetAll(),
                "Id",
                "Name",
                user.RoleId
            );

            ViewBag.Employees = new SelectList(
                _unitOfWork.Employees
                           .GetAllWithDepartment()
                           .OrderBy(e => e.Name),
                "Id",
                "Name",
                user.EmployeeId
            );

            return View(user);
        }

        // =========================
        // Edit - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            // كلمة المرور لا تتغير من صفحة التعديل
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                var existingUser =
                    _unitOfWork.Users.GetById(id);

                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.RoleId = user.RoleId;
                existingUser.EmployeeId = user.EmployeeId;

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(
                _unitOfWork.Roles.GetAll(),
                "Id",
                "Name",
                user.RoleId
            );

            ViewBag.Employees = new SelectList(
                _unitOfWork.Employees
                           .GetAllWithDepartment()
                           .OrderBy(e => e.Name),
                "Id",
                "Name",
                user.EmployeeId
            );

            return View(user);
        }

        // =========================
        // Delete - GET
        // =========================
        public IActionResult Delete(int id)
        {
            var user =
                _unitOfWork.Users
                           .GetByIdWithRoleAndEmployee(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // =========================
        // Delete - POST
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user =
                _unitOfWork.Users.GetById(id);

            if (user != null)
            {
                _unitOfWork.Users.Delete(user);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}