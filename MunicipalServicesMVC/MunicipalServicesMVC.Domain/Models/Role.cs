using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVC.Domain.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "اسم الدور")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        // المستخدمون المرتبطون بهذا الدور
        public ICollection<User>? Users { get; set; }

        // الصلاحيات المرتبطة بهذا الدور
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}