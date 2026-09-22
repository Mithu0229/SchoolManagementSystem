using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.School.Dashboard.Models;
using SchoolManagementSystem.Application.School.Dashboard.Queries;

namespace SchoolManagementSystem.API.Controllers;

public class DashboardController : ProtectedBaseController
{
    [HttpGet("admin-stats")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdminDashboardStatsResponse))]
    public async Task<IResult> GetAdminStats() => await Mediator.Send(new GetAdminDashboardStatsQuery());
}
