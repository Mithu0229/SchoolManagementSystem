using SchoolManagementSystem.Application.GS.Divisions.Models;

namespace SchoolManagementSystem.Application.GS.Divisions.Commands;
public class UpdateDivisionCommand : IHttpRequest
{
    public DivisionRequest Division { get; set; }

}

