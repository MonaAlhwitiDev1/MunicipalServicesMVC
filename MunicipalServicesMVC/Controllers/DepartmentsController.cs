using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVC.DTOs;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // Index
        // =========================
        public IActionResult Index()
        {
            IEnumerable<Department> departments =
                _unitOfWork.Departments.GetAllWithDetails();

            return View(departments);
        }

        // =========================
        // Create - GET
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // Create - POST
        // DTO + Mapping
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateDepartmentDto departmentDto)
        {
            if (ModelState.IsValid)
            {
                Department department = new Department
                {
                    Name = departmentDto.Name,
                    UID = Guid.NewGuid().ToString()
                };

                _unitOfWork.Departments.Add(department);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // =========================
        // Edit - GET
        // UID
        // =========================
        [HttpGet]
        public IActionResult Edit(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return NotFound();
            }

            Department? department =
                _unitOfWork.Departments.GetByUID(uid);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // =========================
        // Edit - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            string uid,
            Department department)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return NotFound();
            }

            Department? oldDepartment =
                _unitOfWork.Departments.GetByUID(uid);

            if (oldDepartment == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                oldDepartment.Name = department.Name;

                _unitOfWork.Departments.Update(oldDepartment);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(oldDepartment);
        }

        // =========================
        // Delete - GET
        // UID
        // =========================
        [HttpGet]
        public IActionResult Delete(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return NotFound();
            }

            Department? department =
                _unitOfWork.Departments.GetByUID(uid);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // =========================
        // Delete - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return NotFound();
            }

            Department? department =
                _unitOfWork.Departments.GetByUID(uid);

            if (department == null)
            {
                return NotFound();
            }

            _unitOfWork.Departments.Delete(department);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}