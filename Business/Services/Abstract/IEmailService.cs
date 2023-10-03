using SendGrid;

namespace Business.Services.Abstract;

public interface IEmailService
{
    Task<Response> SendResetEmailAsync(string toEmail, string message);
    Task<Response> SendEmailForConfirmation(string toEmail, string message);
}
