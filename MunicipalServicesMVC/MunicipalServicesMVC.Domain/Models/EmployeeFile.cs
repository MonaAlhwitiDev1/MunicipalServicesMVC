using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MunicipalServicesMVC.Domain.Models
{
    public class EmployeeFile
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string FileURL { get; set; } = string.Empty;

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }
    }
}