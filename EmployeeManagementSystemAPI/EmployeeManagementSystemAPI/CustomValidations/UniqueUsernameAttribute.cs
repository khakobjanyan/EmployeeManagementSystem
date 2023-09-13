using EmployeeManagementSystemAPI.Context;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemAPI.CustomValidations
{
    public class UniqueUsernameAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            var username = value as string;

            if (dbContext.Users.Any(u => u.UserName == username))
            {
                return new ValidationResult("This username is already in use.");
            }

            return ValidationResult.Success;
        }
    }
}
