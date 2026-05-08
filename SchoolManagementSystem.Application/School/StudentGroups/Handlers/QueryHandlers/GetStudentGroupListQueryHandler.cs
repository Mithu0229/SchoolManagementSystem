using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.StudentGroups.Models;
using SchoolManagementSystem.Application.School.StudentGroups.Queries;

namespace SchoolManagementSystem.Application.School.StudentGroups.Handlers.QueryHandlers;

public class GetStudentGroupListQueryHandler : IHttpRequestHandler<GetStudentGroupListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetStudentGroupListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetStudentGroupListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.StudentGroupRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.GroupName.ToLower().Contains(search) || (x.GroupDetails != null && x.GroupDetails.ToLower().Contains(search)));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            var items = await query.Select(x => new StudentGroupResponse { Id = x.Id, GroupName = x.GroupName, GroupDetails = x.GroupDetails, IsActive = x.IsActive }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<StudentGroupResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<StudentGroupResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
