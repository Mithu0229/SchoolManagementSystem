using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.SchoolStudents.Models;
using SchoolManagementSystem.Application.School.SchoolStudents.Queries;

namespace SchoolManagementSystem.Application.School.SchoolStudents.Handlers.QueryHandlers;

public class GetStudentListQueryHandler : IHttpRequestHandler<GetStudentListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetStudentListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.StudentRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.StudentCode.ToLower().Contains(search) || x.StudentName.ToLower().Contains(search) || (x.MobileNo != null && x.MobileNo.ToLower().Contains(search)));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            var items = await query.Select(x => new StudentResponse
            {
                Id = x.Id,
                StudentCode = x.StudentCode,
                StudentName = x.StudentName,
                DateOfBirth = x.DateOfBirth,
                Gender = x.Gender,
                BloodGroup = x.BloodGroup,
                MobileNo = x.MobileNo,
                Email = x.Email,
                DOBNo = x.DOBNo,
                GuardianNID = x.GuardianNID,
                FatherName = x.FatherName,
                MotherName = x.MotherName,
                GuardianMobileNo = x.GuardianMobileNo,
                PresentAddress = x.PresentAddress,
                PermanentAddress = x.PermanentAddress,
                PhotoPath = x.PhotoPath,
                IsActive = x.IsActive
            }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<StudentResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<StudentResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
