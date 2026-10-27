using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MunicipalServicesMVC.Domain.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public Department? Department { get; set; }
    }
}