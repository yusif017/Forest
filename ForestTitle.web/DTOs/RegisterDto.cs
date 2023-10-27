using System.ComponentModel.DataAnnotations;

namespace ForesTitle.web.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [Compare("Password")]
        public string? PasswordConfirmation { get; set; }

    }
}
