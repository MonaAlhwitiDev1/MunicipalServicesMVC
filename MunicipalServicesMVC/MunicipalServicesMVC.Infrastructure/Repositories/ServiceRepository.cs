using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Infrastructure.Data;
using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly AppDbContext _db;

        public ServiceRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        // جميع الخدمات مع الإدارة
        public IEnumerable<Service> GetAllWithDepartment()
        {
            return _db.Services
                .Include(s => s.Department)
                .ToList();
        }

        // جلب خدمة مع الإدارة
        public Service? GetByIdWithDepartment(int id)
        {
            return _db.Services
                .Include(s => s.Department)
                .FirstOrDefault(s => s.Id == id);
        }
    }
}