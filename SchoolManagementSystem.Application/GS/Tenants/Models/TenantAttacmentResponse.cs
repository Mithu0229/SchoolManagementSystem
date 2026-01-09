namespace SchoolManagementSystem.Application.GS.Tenants.Models;

public class TenantAttacmentResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid AttachmentId { get; set; }
    public Guid AttachmentTypeId { get; set; }
    public string? AttachmentTypeName { get; set; }
    public string? AttachmentName { get; set; }
    public string? OriginalFileName { get; set; }
    public string? TenantName { get; set; }
    public string? AttachedFile { get; set; }
    public string? FileSize { get; set; }
}
