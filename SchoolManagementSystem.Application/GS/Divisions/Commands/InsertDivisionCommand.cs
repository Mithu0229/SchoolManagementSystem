using SchoolManagementSystem.Application.GS.Divisions.Models;

namespace SchoolManagementSystem.Application.GS.Divisions.Commands;
public class InsertDivisionCommand : IHttpRequest
{
    public DivisionRequest Division { get; set; }

}