using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SchoolManagementSystem.Application.Common;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Infrastructure.Common;

public class EmailService : IEmailService
{
    public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, byte[] attachmentBytes, string attachmentName)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Edugates ERP", "admin@edugateserp.com"));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = body
        };

        if (attachmentBytes != null && attachmentBytes.Length > 0)
        {
            builder.Attachments.Add(attachmentName, attachmentBytes, new ContentType("application", "pdf"));
        }

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            // Connect to the server
            await client.ConnectAsync("mail.edugateserp.com", 587, SecureSocketOptions.Auto);

            // Authenticate
            await client.AuthenticateAsync("admin@edugateserp.com", "786#Admin#$@!");

            // Send the email
            await client.SendAsync(message);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}
