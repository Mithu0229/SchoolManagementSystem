using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Models;

public class MenuListResponse
{
    public Guid UserId { get; set; }
    public Guid SitemapId { get; set; }
    public string Name { get; set; }
    public MenuTypes MenuType { get; set; }
    public string FavIcon { get; set; }
    public string PageUrl { get; set; }
    public int SortingOrder { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsFeature { get; set; }
    public bool IsSidebarmenu { get; set; }
    public bool IsActive { get; set; }
    public bool CanView { get; set; }
    public bool CanAdd { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanPreview { get; set; }
    public bool CanExport { get; set; }
    public bool CanPrint { get; set; }
}
