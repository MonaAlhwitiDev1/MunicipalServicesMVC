using MunicipalServicesMVC.Application.DTOs;
using MunicipalServicesMVC.Domain.Models;
namespace MunicipalServicesMVC.Application.Services
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetAllEmployees();

        Employee? GetEmployeeById(int id);

        void AddEmployee(Employee employee);

        void UpdateEmployee(Employee employee);

        void DeleteEmployee(Employee employee);

        IEnumerable<Department> GetAllDepartments();
    }
}