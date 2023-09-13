using EmployeeManagementSystemAPI.Context;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemAPI.CustomValidations
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            var email = value as string;

            if (dbContext.Users.Any(u => u.Email == email))
            {
                return new ValidationResult("This email is already in use.");
            }

            return ValidationResult.Success;
        }
    }
}
