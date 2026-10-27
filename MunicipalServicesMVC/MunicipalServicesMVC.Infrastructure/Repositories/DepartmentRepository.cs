using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Infrastructure.Data;
using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public class DepartmentRepository
        : Repository<Department>, IDepartmentRepository
    {
        private readonly AppDbContext _db;

        public DepartmentRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        // جلب الإدارات مع الموظفين والخدمات
        public IEnumerable<Department> GetAllWithDetails()
        {
            return _db.Departments
                .Include(d => d.Employees)
                .Include(d => d.Services)
                .ToList();
        }

        // جلب إدارة باستخدام UID
        public Department? GetByUID(string uid)
        {
            return _db.Departments
                .Include(d => d.Employees)
                .Include(d => d.Services)
                .FirstOrDefault(d => d.UID == uid);
        }
    }
}