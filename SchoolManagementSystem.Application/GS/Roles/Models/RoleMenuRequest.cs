namespace SchoolManagementSystem.Application.GS.Roles.Models;
public class RoleMenuRequest
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Guid SitemapId { get; set; }

    public bool CanView { get; set; }
    public bool CanAdd { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanPreview { get; set; }
    public bool CanExport { get; set; }
    public bool CanPrint { get; set; }
    public Guid? TenantId { get; set; }
}