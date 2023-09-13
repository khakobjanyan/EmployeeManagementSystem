using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemAPI.Models.Dto
{
    public class LoginModelDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one capital letter, one lowercase letter, and one digit.")]
        public string Password { get; set; }
    }
}
