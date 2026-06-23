using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Admissions.Models;
using SchoolManagementSystem.Application.School.Admissions.Queries;

namespace SchoolManagementSystem.Application.School.Admissions.Handlers.QueryHandlers;

public class GetAdmissionByIdQueryHandler : IHttpRequestHandler<GetAdmissionByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAdmissionByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetAdmissionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<AdmissionResponse>(StatusCodes.Status406NotAcceptable);
            var response = await _unitOfWork.AdmissionRepository.GetAllNoneDeleted(true).Where(x => x.Id == request.Id).Select(x => new AdmissionResponse
            {
                Id = x.Id,
                AdmissionDate = x.AdmissionDate,
                StudentId = x.StudentId,
                StudentName = x.Student.FullName,//.StudentName,
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
                RollNo = x.RollNo,
                IsPassed = x.IsPassed,
                IsCancelled = x.IsCancelled,
                IsActive = x.IsActive
            }).FirstOrDefaultAsync(cancellationToken);
            if (response is null) return Result.Fail<AdmissionResponse>(StatusCodes.Status404NotFound);
            return Result.Success(response);
        }
        catch (Exception ex) { return Result.Fail<AdmissionResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
