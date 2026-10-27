using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Service> services =
                _unitOfWork.Services.GetAllWithDepartment();

            return View(services);
        }

        public IActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(
                _unitOfWork.Departments.GetAll(),
                "Id",
                "Name"
            );

            return View();
        }

        public IActionResult Edit(int id)
        {
            Service? service =
                _unitOfWork.Services.GetById(id);

            if (service == null)
            {
                return NotFound();
            }

            ViewBag.DepartmentId = new SelectList(
                _unitOfWork.Departments.GetAll(),
                "Id",
                "Name",
                service.DepartmentId
            );

            return View(service);
        }

        public IActionResult Delete(int id)
        {
            Service? service =
                _unitOfWork.Services.GetByIdWithDepartment(id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Services.Add(service);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.DepartmentId = new SelectList(
                _unitOfWork.Departments.GetAll(),
                "Id",
                "Name",
                service.DepartmentId
            );

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Services.Update(service);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.DepartmentId = new SelectList(
                _unitOfWork.Departments.GetAll(),
                "Id",
                "Name",
                service.DepartmentId
            );

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Service service)
        {
            Service? existingService =
                _unitOfWork.Services.GetById(service.Id);

            if (existingService == null)
            {
                return NotFound();
            }

            _unitOfWork.Services.Delete(existingService);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}