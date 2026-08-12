using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Shifts.Models;
using SchoolManagementSystem.Application.School.Students.Models;
using SchoolManagementSystem.Application.School.Students.Queries;

namespace SchoolManagementSystem.Application.School.Students.Handlers.QueryHandlers;

public class GetStudentUserListQueryHandler : IHttpRequestHandler<GetStudentUserListQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentUserListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetStudentUserListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = from s in _unitOfWork.StudentInfoRepository.GetAllNoneDeleted(false,true)
                        join u in _unitOfWork.UserRepository.GetAllNoneDeleted(false, true) on s.Id equals u.StudentId into su
                        from u in su.DefaultIfEmpty()
                        select new StudentUserResponse
                        {
                            StudentId = s.Id,
                            StdCID = s.StdCID,
                            FullName = s.FullName,
                            StudentPhone = s.StudentPhone,
                            StudentEmail = s.StudentEmail,
                            IsActive = u != null ? u.IsActive : false,
                            UserId = u != null ? u.Id : Guid.Empty
                        };

            if (!string.IsNullOrEmpty(request.PagedRequest.Search))
            {
                var search = request.PagedRequest.Search.ToLower();
                query = query.Where(x => x.FullName.ToLower().Contains(search) || 
                                         x.StdCID.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            //var items = await query
            //    .OrderBy(x => x.FullName)
            //    .Skip((request.PagedRequest.Page - 1) * (int)request.PagedRequest.PageSize)
            //    .Take((int)request.PagedRequest.PageSize)
            //    .ToListAsync(cancellationToken);

            var totalRecord = await query.CountAsync(cancellationToken);
            if (request.PagedRequest.Page > 0 && request.PagedRequest.PageSize > 0) query = query.Skip((request.PagedRequest.Page - 1) * request.PagedRequest.PageSize).Take(request.PagedRequest.PageSize);
            var items = await query.Select(x => new StudentUserResponse { StudentId = x.StudentId, StdCID = x.StdCID,StudentPhone =x.StudentPhone,FullName=x.FullName,StudentEmail=x.StudentEmail ,IsActive = x.IsActive }).ToListAsync(cancellationToken);
            //return Result.Success(new PagedResult<StudentUserResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
            return Result.Success(new PagedResult<StudentUserResponse> { Items = items, TotalRecord = totalCount, Page = request.PagedRequest.Page, PageSize = request.PagedRequest.PageSize });
        }
        catch (Exception ex)
        {
            return Result.Fail<PagedResult<StudentUserResponse>>(StatusCodes.Status500InternalServerError);
        }
    }
}
