using EmployeeManagementSystemAPI.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [UniqueUsername]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [UniqueEmail]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one capital letter, one lowercase letter, and one digit.")]
        public string Password { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpTime { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime ResetPasswordExpired { get; set; }
    }
}
