using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Common;

public interface IEmailService
{
    Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, byte[] attachmentBytes, string attachmentName);
}
