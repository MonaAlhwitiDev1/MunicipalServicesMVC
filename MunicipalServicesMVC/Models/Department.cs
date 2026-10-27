using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVC.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; }
            = new List<Employee>();

        public ICollection<Service> Services { get; set; }
            = new List<Service>();
    }
}