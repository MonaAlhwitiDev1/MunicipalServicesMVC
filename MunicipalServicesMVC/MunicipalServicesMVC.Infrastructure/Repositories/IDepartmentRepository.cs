using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        // جلب الإدارات مع الموظفين والخدمات
        IEnumerable<Department> GetAllWithDetails();

        // جلب إدارة باستخدام UID
        Department? GetByUID(string uid);
    }
}