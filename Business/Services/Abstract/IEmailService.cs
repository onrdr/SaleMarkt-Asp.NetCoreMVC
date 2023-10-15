using SendGrid;

namespace Business.Services.Abstract;

public interface IEmailService
{
    Task<Response> SendResetEmailAsync(string toEmail, string message);
    Task<Response> SendEmailForConfirmationAsync(string toEmail, string message);
}
