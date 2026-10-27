using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize(Roles = "مدير النظام")]
    public class PermissionsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // عرض جميع الصلاحيات
        public IActionResult Index()
        {
            var permissions =
                _unitOfWork.Permissions.GetAll();

            return View(permissions);
        }

        // فتح صفحة إضافة صلاحية جديدة
        public IActionResult Create()
        {
            return View();
        }

        // حفظ الصلاحية الجديدة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Permissions.Add(permission);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(permission);
        }

        // فتح صفحة تعديل الصلاحية
        public IActionResult Edit(int id)
        {
            var permission =
                _unitOfWork.Permissions.GetById(id);

            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // حفظ التعديلات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Permission permission)
        {
            if (id != permission.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Permissions.Update(permission);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(permission);
        }

        // فتح صفحة تأكيد الحذف
        public IActionResult Delete(int id)
        {
            var permission =
                _unitOfWork.Permissions.GetById(id);

            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // حذف الصلاحية
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var permission =
                _unitOfWork.Permissions.GetById(id);

            if (permission != null)
            {
                _unitOfWork.Permissions.Delete(permission);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}