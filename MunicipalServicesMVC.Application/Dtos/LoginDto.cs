using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVC.Application.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "أدخل البريد الإلكتروني")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "أدخل كلمة المرور")]
        public string Password { get; set; } = string.Empty;
    }
}