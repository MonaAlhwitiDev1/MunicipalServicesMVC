using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Infrastructure.Data;
using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _db;

        public EmployeeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        // =========================
        // الموظفين
        // =========================

        public IEnumerable<Employee> GetAllWithDepartment()
        {
            return _db.Employees
                .Include(e => e.Department)
                .ToList();
        }

        public Employee? GetByIdWithDepartment(int id)
        {
            return _db.Employees
                .Include(e => e.Department)
                .FirstOrDefault(e => e.Id == id);
        }

        // =========================
        // ملفات الموظفين
        // =========================

        public Dictionary<int, string> GetLatestImages()
        {
            return _db.EmployeeFiles
                .GroupBy(f => f.EmployeeId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(f => f.Id)
                          .First()
                          .FileURL
                );
        }

        public IEnumerable<EmployeeFile> GetFiles(int employeeId)
        {
            return _db.EmployeeFiles
                .Where(f => f.EmployeeId == employeeId)
                .ToList();
        }

        public EmployeeFile? GetFileById(int id)
        {
            return _db.EmployeeFiles
                .FirstOrDefault(f => f.Id == id);
        }

        public void AddFile(EmployeeFile employeeFile)
        {
            _db.EmployeeFiles.Add(employeeFile);
        }

        public void DeleteFile(EmployeeFile employeeFile)
        {
            _db.EmployeeFiles.Remove(employeeFile);
        }
    }
}