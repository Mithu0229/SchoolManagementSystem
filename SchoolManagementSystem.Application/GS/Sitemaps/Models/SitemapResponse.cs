using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Models;

public class SitemapResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FavIcon { get; set; }
    public string PageUrl { get; set; }
    public int SortingOrder { get; set; }
    public bool IsFeature { get; set; }
    public bool IsSidebarmenu { get; set; }
    public MenuTypes MenuType { get; set; }
    public string? MenuTypeName { get; set; }
    public MenuAccessTypes MenuAccessType { get; set; }
    public string? AccessTypeName { get; set; }
    public bool IsActive { get; set; }
    public Guid? ParentId { get; set; }
    public string? ParentName { get; set; }
    public string? NameWithParent { get; set; }
}
