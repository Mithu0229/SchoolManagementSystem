using SchoolManagementSystem.Application.School.Students.Models;
using SchoolManagementSystem.Application.School.Students.Queries;

namespace SchoolManagementSystem.Application.School.Students.Handlers.QueryHandlers;
public class GetStudentListQueryHandler : IHttpRequestHandler<GetStudentListQuery>
{
    private IPagedService _pagedService;
    public GetStudentListQueryHandler(IPagedService pagedService)
    {
        _pagedService = pagedService;
    }
    public async Task<IResult> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _pagedService.GetPagedAsync<StudentInfoResponse>("dbo.sp_GetStudentList", "dbo.sp_GetStudentListCount", request.PagedRequest, false, false);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<IList<StudentInfoResponse>>(StatusCodes.Status500InternalServerError);
        }
    }

}
