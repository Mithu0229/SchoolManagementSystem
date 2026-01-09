using SchoolManagementSystem.Application.GS.Sitemaps.Models;
using SchoolManagementSystem.Application.GS.Sitemaps.Queries;
using System.Data;

namespace SchoolManagementSystem.Application.GS.Sitemaps.Handlers.QueryHandlers;

public class GetAllMenuListQueryHandler : IHttpRequestHandler<GetAllMenuListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAllMenuListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<IResult> Handle(GetAllMenuListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var parameters = new Dictionary<string, object>
{
    { "@UserId", request.Id! }
};

            var menuList = (await _unitOfWork.DapperCommandQuery.GetDataListAsync<MenuListResponse>(
                "sp_GetUserWiseMenuPermission",
                parameters,
                System.Data.CommandType.StoredProcedure
            )).ToList();


            var menuTree = BuildMenuTree(menuList);
            return Result.Success(menuTree);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<IList<MenuListResponse>>(StatusCodes.Status500InternalServerError);
        }
    }


    private List<MenuTreeResponse> BuildMenuTree(List<MenuListResponse> flatMenuList)
    {
        // Create a map of SitemapId to MenuListResponse for lookup
        var validItems = flatMenuList
            .Where(m => m.CanView && m.SitemapId != Guid.Empty)
            .ToList();

        // Create dictionary for quick lookup by SitemapId
        var itemLookup = new Dictionary<Guid, MenuTreeItem>();

        foreach (var m in validItems)
        {
            if (!string.IsNullOrEmpty(m.Name) && !string.IsNullOrEmpty(m.FavIcon) && !string.IsNullOrEmpty(m.PageUrl))
            {
                itemLookup[m.SitemapId] = new MenuTreeItem
                {
                    SitemapId = m.SitemapId,
                    MenuType = m.MenuType.ToString(),
                    Label = m.Name,
                    Icon = m.FavIcon,
                    RouterLink = new List<string> { m.PageUrl },
                    Items = null, // Do not initialize with empty list
                    IsSidebarmenu = m.IsSidebarmenu,
                    CanView = m.CanView,
                    CanAdd = m.CanAdd,
                    CanEdit = m.CanEdit,
                    CanDelete = m.CanDelete,
                    CanPreview = m.CanPreview,
                    CanExport = m.CanExport,
                    CanPrint = m.CanPrint
                };
            }
        }

        var tree = new List<MenuTreeResponse>();
        var processedIds = new HashSet<Guid>();

        // Top-level items
        var topLevelItems = validItems
            .Where(m => m.ParentId == null)
            .OrderBy(m => m.SortingOrder)
            .ToList();

        foreach (var topLevel in topLevelItems)
        {
            if (!itemLookup.TryGetValue(topLevel.SitemapId, out var topLevelItem)) continue;

            var group = new MenuTreeResponse { Items = new List<MenuTreeItem> { topLevelItem } };
            processedIds.Add(topLevel.SitemapId);

            AddChildren(topLevel.SitemapId, validItems, itemLookup, topLevelItem, processedIds);

            tree.Add(group);
        }

        // Orphaned items (not already processed)
        var orphanedItems = validItems
            .Where(m => !processedIds.Contains(m.SitemapId))
            .OrderBy(m => m.SortingOrder)
            .ToList();

        foreach (var orphan in orphanedItems)
        {
            if (!itemLookup.TryGetValue(orphan.SitemapId, out var orphanItem)) continue;

            var group = new MenuTreeResponse { Items = new List<MenuTreeItem> { orphanItem } };
            processedIds.Add(orphan.SitemapId);
            AddChildren(orphan.SitemapId, validItems, itemLookup, orphanItem, processedIds);

            tree.Add(group);
        }

        return tree;
    }

    private void AddChildren(Guid parentId, List<MenuListResponse> flatMenuList,
        Dictionary<Guid, MenuTreeItem> itemLookup, MenuTreeItem parentItem, HashSet<Guid> processedIds)
    {
        var children = flatMenuList
            .Where(m => m.ParentId == parentId)
            .OrderBy(m => m.SortingOrder)
            .ToList();

        foreach (var child in children)
        {
            if (processedIds.Contains(child.SitemapId) || !itemLookup.ContainsKey(child.SitemapId)) continue;

            var childItem = itemLookup[child.SitemapId];

            // Lazy initialize only if children exist
            parentItem.Items ??= new List<MenuTreeItem>();
            parentItem.Items.Add(childItem);
            processedIds.Add(child.SitemapId);

            AddChildren(child.SitemapId, flatMenuList, itemLookup, childItem, processedIds);
        }

        // Remove empty Items list if no children were actually added
        if (parentItem.Items != null && parentItem.Items.Count == 0)
        {
            parentItem.Items = null;
        }
    }




}
