using System.Net;
using System.Net.Mail;
using ArcheryAcademy.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ArcheryAcademy.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }
    
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var host = _config["Email:Host"];
        var port = int.Parse(_config["Email:Port"]!);
        var username = _config["Email:Username"];
        var password = _config["Email:Password"];
        var from = _config["Email:From"];

        var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        var mail = new MailMessage(from!, toEmail, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(mail);
    }


    public async Task SendAsync(string to, string subject, string body)
    {
        var smtp = new SmtpClient
        {
            Host = _config["Email:Host"],
            Port = int.Parse(_config["Email:Port"]),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _config["Email:Username"],
                _config["Email:Password"]
            )
        };

        var message = new MailMessage
        {
            From = new MailAddress(_config["Email:From"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(to);

        await smtp.SendMailAsync(message);
    }
}