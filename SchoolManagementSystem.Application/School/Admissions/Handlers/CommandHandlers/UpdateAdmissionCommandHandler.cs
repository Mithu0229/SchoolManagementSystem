using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Admissions.Commands;
using SchoolManagementSystem.Application.School.Admissions.Models;

namespace SchoolManagementSystem.Application.School.Admissions.Handlers.CommandHandlers;

public class UpdateAdmissionCommandHandler : IHttpRequestHandler<UpdateAdmissionCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateAdmissionCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateAdmissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.Admission.Id == Guid.Empty) return Result.Fail<AdmissionResponse>(StatusCodes.Status406NotAcceptable);
            request.Admission.RollNo = request.Admission.RollNo.Trim();
            var entity = await _unitOfWork.AdmissionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Admission.Id);
            if (entity is null) return Result.Fail<AdmissionResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.AdmissionRepository.GetAllNoneDeleted()
                .Where(x => x.Id != request.Admission.Id && x.BranchId == request.Admission.BranchId && x.AcademicSessionId == request.Admission.AcademicSessionId && x.ClassId == request.Admission.ClassId && x.RollNo.ToLower() == request.Admission.RollNo.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Admission roll already exists!");
            entity.AdmissionDate = request.Admission.AdmissionDate;
            entity.StudentId = request.Admission.StudentId;
            entity.BranchId = request.Admission.BranchId;
            entity.AcademicSessionId = request.Admission.AcademicSessionId;
            entity.ClassId = request.Admission.ClassId;
            entity.SectionId = request.Admission.SectionId;
            entity.ShiftId = request.Admission.ShiftId;
            entity.GroupId = request.Admission.GroupId;
            entity.TeacherId = request.Admission.TeacherId;
            entity.RollNo = request.Admission.RollNo;
            entity.IsPassed = request.Admission.IsPassed;
            entity.IsCancelled = request.Admission.IsCancelled;
            entity.IsActive = request.Admission.IsActive;
            await _unitOfWork.AdmissionRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<AdmissionResponse>(), "Admission " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<AdmissionResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
