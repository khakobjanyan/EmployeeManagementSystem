using EmployeeManagementSystemAPI.Models;

namespace EmployeeManagementSystemAPI.Utility.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);

    }
}
