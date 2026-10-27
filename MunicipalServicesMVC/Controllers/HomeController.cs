using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVC.Domain.Models;
using ErrorViewModel = MunicipalServicesMVC.Models.ErrorViewModel;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // عرض لوحة التحكم
        public IActionResult Index()
        {
            var departments = _unitOfWork.Departments
                .GetAll()
                .ToList();

            var employees = _unitOfWork.Employees
                .GetAllWithDepartment()
                .ToList();

            var services = _unitOfWork.Services
                .GetAllWithDepartment()
                .ToList();

            // الإحصائيات
            ViewBag.DepartmentsCount = departments.Count;
            ViewBag.EmployeesCount = employees.Count;
            ViewBag.ServicesCount = services.Count;

            // بيانات أحدث السجلات حسب المعرف
            ViewBag.LatestEmployees = employees
                .OrderByDescending(e => e.Id)
                .Take(5)
                .ToList();

            ViewBag.LatestServices = services
                .OrderByDescending(s => s.Id)
                .Take(5)
                .ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}