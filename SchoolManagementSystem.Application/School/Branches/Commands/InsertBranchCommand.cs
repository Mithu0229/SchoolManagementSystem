using SchoolManagementSystem.Application.School.Branches.Models;

namespace SchoolManagementSystem.Application.School.Branches.Commands;

public class InsertBranchCommand : IHttpRequest
{
    public BranchRequest Branch { get; set; }
}
