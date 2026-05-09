using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Admissions.Commands;
using SchoolManagementSystem.Application.School.Admissions.Models;

namespace SchoolManagementSystem.Application.School.Admissions.Handlers.CommandHandlers;

public class InsertAdmissionCommandHandler : IHttpRequestHandler<InsertAdmissionCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertAdmissionCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertAdmissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<AdmissionResponse>(StatusCodes.Status406NotAcceptable);
            request.Admission.RollNo = request.Admission.RollNo.Trim();
            var duplicate = await _unitOfWork.AdmissionRepository.GetAllNoneDeleted()
                .Where(x => x.BranchId == request.Admission.BranchId && x.AcademicSessionId == request.Admission.AcademicSessionId && x.ClassId == request.Admission.ClassId && x.RollNo.ToLower() == request.Admission.RollNo.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Admission roll already exists!");
            var entity = request.Admission.Adapt<Admission>();
            await _unitOfWork.AdmissionRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<AdmissionResponse>(), "Admission " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<AdmissionResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
