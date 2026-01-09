namespace SchoolManagementSystem.Application.GS.Roles.Models;
public class RoleMenuResponse
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
    public string? RoleName { get; set; }
    public string? SiteMapName { get; set; }
    public string? PageUrl { get; set; }
    public string? NameWithParent { get; set; }
    public string? TenantName { get; set; }
}
