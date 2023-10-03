using System.Net.Mail;
using System.Net;
using Business.Services.Abstract;
using Models.Smtp;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Errors.Model;

namespace Business.Services.Concrete;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task<Response> SendEmailForConfirmation(string toEmail, string message)
    {
        return await Execute(toEmail, message);
    }

    public async Task<Response> SendResetEmailAsync(string toEmail, string message)
    {
        return await Execute(toEmail, message);
    }

    private async Task<Response> Execute(string toEmail, string message)
    {
        var apiKey = _smtpSettings.ApiKey;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_smtpSettings.FromEmail);
        var subject = _smtpSettings.PasswordResetSubject;
        var to = new EmailAddress(toEmail);
        var plainTextContent = "SaleMarkt reset password";
        var htmlContent = message;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);

        return response;
    }
}
