using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        // جميع الخدمات مع الإدارة
        IEnumerable<Service> GetAllWithDepartment();

        // جلب خدمة مع الإدارة
        Service? GetByIdWithDepartment(int id);
    }
}