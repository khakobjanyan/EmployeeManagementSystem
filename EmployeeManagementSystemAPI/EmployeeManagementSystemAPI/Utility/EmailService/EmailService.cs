using EmployeeManagementSystemAPI.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace EmployeeManagementSystemAPI.Utility.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendEmail(EmailModel emailModel)
        {
            var emailMessage = new MimeMessage();
            var from = configuration["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress("Employee Management system", from));
            emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    client.Connect(configuration["EmailSettings:SmtpServer"], 465, true);
                    client.Authenticate(configuration["EmailSettings:From"], configuration["EmailSettings:Password"]);
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    throw new BadHttpRequestException(ex.Message);
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }
    }
}
