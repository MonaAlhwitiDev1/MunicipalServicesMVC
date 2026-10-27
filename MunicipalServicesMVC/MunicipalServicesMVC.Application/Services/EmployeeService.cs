using MunicipalServicesMVC.Application.DTOs;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.DTOs;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees()
        {
            var employees = _unitOfWork.Employees.GetAllWithDepartment();

            return employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                JobTitle = e.JobTitle,
                DepartmentName = e.Department != null
                    ? e.Department.Name
                    : string.Empty
            }).ToList();
        }

        public Employee? GetEmployeeById(int id)
        {
            return _unitOfWork.Employees.GetByIdWithDepartment(id);
        }

        public void AddEmployee(Employee employee)
        {
            _unitOfWork.Employees.Add(employee);
            _unitOfWork.Save();
        }

        public void UpdateEmployee(Employee employee)
        {
            _unitOfWork.Employees.Update(employee);
            _unitOfWork.Save();
        }

        public void DeleteEmployee(Employee employee)
        {
            _unitOfWork.Employees.Delete(employee);
            _unitOfWork.Save();
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return _unitOfWork.Departments.GetAll();
        }
    }
}