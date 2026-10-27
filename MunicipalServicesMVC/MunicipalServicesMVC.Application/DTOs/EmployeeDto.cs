namespace MunicipalServicesMVC.Application.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;
    }
}
