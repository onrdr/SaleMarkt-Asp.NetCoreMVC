using System.Net.Mail;
using System.Net;
using Business.Services.Abstract;
using Models.Smtp;
using Microsoft.Extensions.Options;

namespace Business.Services.Concrete;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailForConfirmation(string toEmail, string message)
    {
        var subject = _smtpSettings.ConfirmEmailSubject;
        await Send(toEmail, message, subject);
    }

    public async Task SendResetEmailAsync(string toEmail, string message)
    {
        var subject = _smtpSettings.PasswordResetSubject;
        await Send(toEmail, message, subject);
    }

    private async Task Send(string toEmail, string message, string subject)
    {
        using var client = new SmtpClient()
        {
            Host = _smtpSettings.Host,
            Port = _smtpSettings.Port,
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = _smtpSettings.EnableSsl,
            UseDefaultCredentials = false
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpSettings.FromEmail),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        await client.SendMailAsync(mailMessage);
    }
}
