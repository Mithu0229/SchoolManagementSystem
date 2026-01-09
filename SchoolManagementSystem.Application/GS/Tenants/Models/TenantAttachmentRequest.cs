namespace SchoolManagementSystem.Application.GS.Tenants.Models;

public class TenantAttachmentRequest
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid AttachmentId { get; set; }
    public Guid AttachmentTypeId { get; set; }
    public string? AttachmentName { get; set; }
    public string? AttachedFile { get; set; }
}
