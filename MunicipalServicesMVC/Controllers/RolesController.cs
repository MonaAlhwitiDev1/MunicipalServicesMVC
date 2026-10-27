using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVC.Domain.Models;
using RolePermissionsViewModel = MunicipalServicesMVC.Models.RolePermissionsViewModel;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize(Roles = "مدير النظام")]
    public class RolesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // عرض جميع الأدوار
        public IActionResult Index()
        {
            var roles = _unitOfWork.Roles.GetAll();

            return View(roles);
        }

        // فتح صفحة إضافة دور جديد
        public IActionResult Create()
        {
            return View();
        }

        // حفظ الدور الجديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Roles.Add(role);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }

        // فتح صفحة تعديل الدور
        public IActionResult Edit(int id)
        {
            var role = _unitOfWork.Roles.GetById(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // حفظ التعديلات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Role role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Roles.Update(role);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }

        // فتح صفحة تأكيد الحذف
        public IActionResult Delete(int id)
        {
            var role = _unitOfWork.Roles.GetById(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // حذف الدور
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var role = _unitOfWork.Roles.GetById(id);

            if (role != null)
            {
                _unitOfWork.Roles.Delete(role);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        // فتح صفحة ربط الدور بالصلاحيات
        public IActionResult Permissions(int id)
        {
            var role = _unitOfWork.Roles.GetById(id);

            if (role == null)
            {
                return NotFound();
            }

            var selectedPermissionIds =
                _unitOfWork.Roles.GetSelectedPermissionIds(id);

            var viewModel = new RolePermissionsViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Permissions = _unitOfWork.Permissions.GetAll().ToList(),
                SelectedPermissionIds = selectedPermissionIds
            };

            return View(viewModel);
        }

        // حفظ صلاحيات الدور
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Permissions(RolePermissionsViewModel model)
        {
            var oldPermissions =
                _unitOfWork.Roles
                           .GetRolePermissions(model.RoleId);

            _unitOfWork.Roles
                       .DeleteRolePermissions(oldPermissions);

            if (model.SelectedPermissionIds != null)
            {
                foreach (var permissionId in model.SelectedPermissionIds)
                {
                    _unitOfWork.Roles.AddRolePermission(
                        new RolePermission
                        {
                            RoleId = model.RoleId,
                            PermissionId = permissionId
                        }
                    );
                }
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}