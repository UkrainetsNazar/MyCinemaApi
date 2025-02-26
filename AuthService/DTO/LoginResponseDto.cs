namespace AuthService.DTO
{
    public class LoginResponseDto
    {
        public string? AccessToken { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
