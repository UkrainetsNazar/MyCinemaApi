using System.ComponentModel.DataAnnotations;

namespace AuthService.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Некоректний формат email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [MinLength(6, ErrorMessage = "Пароль має містити мінімум 6 символів")]
        public string? Password { get; set; }
    }
}
