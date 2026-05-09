using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.SchoolStudents.Commands;
using SchoolManagementSystem.Application.School.SchoolStudents.Models;

namespace SchoolManagementSystem.Application.School.SchoolStudents.Handlers.CommandHandlers;

public class UpdateStudentCommandHandler : IHttpRequestHandler<UpdateStudentCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateStudentCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.Student.Id == Guid.Empty) return Result.Fail<StudentResponse>(StatusCodes.Status406NotAcceptable);
            request.Student.StudentCode = request.Student.StudentCode.Trim();
            request.Student.StudentName = request.Student.StudentName.Trim();
            var entity = await _unitOfWork.StudentRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Student.Id);
            if (entity is null) return Result.Fail<StudentResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.StudentRepository.GetAllNoneDeleted().Where(x => x.Id != request.Student.Id && x.StudentCode.ToLower() == request.Student.StudentCode.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Student code already exists!");
            entity.StudentCode = request.Student.StudentCode;
            entity.StudentName = request.Student.StudentName;
            entity.DateOfBirth = request.Student.DateOfBirth;
            entity.Gender = request.Student.Gender;
            entity.BloodGroup = request.Student.BloodGroup;
            entity.MobileNo = request.Student.MobileNo;
            entity.Email = request.Student.Email;
            entity.DOBNo = request.Student.DOBNo;
            entity.GuardianNID = request.Student.GuardianNID;
            entity.FatherName = request.Student.FatherName;
            entity.MotherName = request.Student.MotherName;
            entity.GuardianMobileNo = request.Student.GuardianMobileNo;
            entity.PresentAddress = request.Student.PresentAddress;
            entity.PermanentAddress = request.Student.PermanentAddress;
            entity.PhotoPath = request.Student.PhotoPath;
            entity.IsActive = request.Student.IsActive;
            await _unitOfWork.StudentRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<StudentResponse>(), "Student " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<StudentResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
