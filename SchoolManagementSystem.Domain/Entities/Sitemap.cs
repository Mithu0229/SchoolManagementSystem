using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Domain.Entities;

public class Sitemap : AuditableEntity
{
    public string Name { get; set; }
    public string FavIcon { get; set; }
    public string PageUrl { get; set; }
    public int SortingOrder { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsFeature { get; set; }
    //Menu Type Will be added in future
    //Admin, User, Customer, etc.
    public bool IsSidebarmenu { get; set; }
    public MenuTypes MenuType { get; set; }
    public MenuAccessTypes MenuAccessType { get; set; }
    public virtual Sitemap Parent { get; set; }
    public virtual ICollection<Sitemap> SitemapList { get; set; }
    public virtual ICollection<RoleMenu> RoleMenuList { get; set; }
}
