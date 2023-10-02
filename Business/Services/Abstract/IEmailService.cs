namespace Business.Services.Abstract;

public interface IEmailService
{
    Task SendResetEmailAsync(string toEmail, string message);
    Task SendEmailForConfirmation(string toEmail, string message);
}
