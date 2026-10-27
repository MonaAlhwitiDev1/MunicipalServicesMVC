using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IEnumerable<Employee> GetAllWithDepartment();

        Employee? GetByIdWithDepartment(int id);

        Dictionary<int, string> GetLatestImages();

        IEnumerable<EmployeeFile> GetFiles(int employeeId);

        EmployeeFile? GetFileById(int id);

        void AddFile(EmployeeFile employeeFile);

        void DeleteFile(EmployeeFile employeeFile);
    }
}