using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVC.Application.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "أدخل الاسم")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "أدخل البريد الإلكتروني")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "أدخل كلمة المرور")]
        [MinLength(
            6,
            ErrorMessage = "كلمة المرور يجب ألا تقل عن 6 أحرف"
        )]
        public string Password { get; set; } = string.Empty;
    }
}