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
        [Required]
        [MinLength(3, ErrorMessage = "Ім'я користувача має містити мінімум 5 символів")]
        public string? UserName { get; set; }
        public string? Role { get; set; }
    }
}
