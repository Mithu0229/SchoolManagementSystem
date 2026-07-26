using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Admissions.Models;
using SchoolManagementSystem.Application.School.Admissions.Queries;

namespace SchoolManagementSystem.Application.School.Admissions.Handlers.QueryHandlers;

public class GetAdmissionListQueryHandler : IHttpRequestHandler<GetAdmissionListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAdmissionListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetAdmissionListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.AdmissionRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.RollNo.ToLower().Contains(search) || x.Student.FullName.ToLower().Contains(search) || x.Student.StdCID.ToLower().Contains(search));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            var items = await query.Select(x => new AdmissionResponse
            {
                Id = x.Id,
                AdmissionDate = x.AdmissionDate,
                StdCID = x.Student.StdCID,
                StudentId = x.StudentId,
                StudentName = x.Student.FullName,
                BranchId = x.BranchId,
                BranchName = x.Branch.BranchName,
                AcademicSessionId = x.AcademicSessionId,
                SessionName = x.AcademicSession.SessionName,
                ClassId = x.ClassId,
                ClassName = x.Class.ClassName,
                SectionId = x.SectionId,
                SectionName = x.Section == null ? null : x.Section.SectionName,
                ShiftId = x.ShiftId,
                ShiftName = x.Shift == null ? null : x.Shift.ShiftName,
                GroupId = x.GroupId,
                GroupName = x.Group == null ? null : x.Group.GroupName,
                TeacherId = x.TeacherId,
                TeacherName = x.Teacher == null ? null : x.Teacher.Name,
                RollNo = x.RollNo,
                IsPassed = x.IsPassed,
                IsCancelled = x.IsCancelled,
                IsActive = x.IsActive
            }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<AdmissionResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<AdmissionResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
