using System.ComponentModel.DataAnnotations;

namespace dotnet_task_manager_api.Models
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; } = "";

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = "";
    }

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }

    public class AuthResponse
    {
        public string Token { get; set; } = "";

        public DateTime ExpiresAt { get; set; }
    }
}
