using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.GS.Sitemaps.Commands;
using SchoolManagementSystem.Application.GS.Sitemaps.Models;
using SchoolManagementSystem.Application.GS.Sitemaps.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class SitemapController : PublicBaseController
{

    [HttpPost(("get-menu-list"))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SitemapResponse))]
    public async Task<IResult> Get([FromBody] PagedRequest request)
    {
        return await Mediator.Send(new GetAllSitemapQuery() { MenuPaged = request });//ok
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SitemapResponse))]
    public async Task<IResult> Get(Guid id)
    {
        return await Mediator.Send(new GetSitemapByIdQuery(id));//ok
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SitemapResponse))]
    public async Task<IResult> Post([FromBody] SitemapRequest request)
    {
        InsertSitemapCommand cmd = new InsertSitemapCommand() { Sitemap = request };
        return await Mediator.Send(cmd);//ok
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SitemapResponse))]
    public async Task<IResult> Put([FromBody] SitemapRequest request)
    {
        UpdateSitemapCommand cmd = new UpdateSitemapCommand() { Sitemap = request };
        return await Mediator.Send(cmd);

    }

    [HttpGet("get-all-menu-list/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SitemapResponse))]
    public async Task<IResult> GetAllMenuList(Guid id)
    {
        return await Mediator.Send(new GetAllMenuListQuery(id));//ok
    }

    [HttpGet("get-all-menu-list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SitemapResponse))]
    public async Task<IResult> GetMenuList()
    {
        return await Mediator.Send(new GetMenuListQuery());//ok
    }
    [HttpGet("get-parent-menu-list")]
    public async Task<IResult> GetParentMenuList()
    {
        return await Mediator.Send(new GetParentMenuListQuery());//ok
    }

    [HttpGet("get-feature-list")]
    public async Task<IResult> GetFeatureList()
    {
        return await Mediator.Send(new GetFeatureListQuery());//ok
    }

}

