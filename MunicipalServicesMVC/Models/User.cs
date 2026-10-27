using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVC.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "اسم المستخدم")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "الدور")]
        public int RoleId { get; set; }

        public Role? Role { get; set; }

        // ربط حساب المستخدم بالموظف
        [Display(Name = "الموظف")]
        public int? EmployeeId { get; set; }

        public Employee? Employee { get; set; }
    }
}