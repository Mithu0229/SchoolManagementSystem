using System.Text.Json.Serialization;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Models
{
    public class MenuTreeResponse
    {
        public List<MenuTreeItem> Items { get; set; } = new List<MenuTreeItem>();

    }

    public class MenuTreeItem
    {
        public Guid SitemapId { get; set; }
        public string MenuType { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public List<string> RouterLink { get; set; }
        public bool IsSidebarmenu { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanPreview { get; set; }
        public bool CanExport { get; set; }
        public bool CanPrint { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<MenuTreeItem>? Items { get; set; }
    }
}
