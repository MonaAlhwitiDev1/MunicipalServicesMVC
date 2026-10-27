using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVC.Domain.Models
{
    public class Permission
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "اسم الصلاحية")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}