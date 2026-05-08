using SchoolManagementSystem.Application.School.FeeHeads.Models;

namespace SchoolManagementSystem.Application.School.FeeHeads.Commands;

public class InsertFeeHeadCommand : IHttpRequest
{
    public FeeHeadRequest FeeHead { get; set; }
}
