namespace SchoolManagementSystem.Application.GS.Sitemaps.Commands;

public record DeleteSitemapCommand(Guid id) : IHttpRequest;
