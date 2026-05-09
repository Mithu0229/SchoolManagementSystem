using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.SchoolStudents.Commands;
using SchoolManagementSystem.Application.School.SchoolStudents.Models;

namespace SchoolManagementSystem.Application.School.SchoolStudents.Handlers.CommandHandlers;

public class InsertStudentCommandHandler : IHttpRequestHandler<InsertStudentCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertStudentCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<StudentResponse>(StatusCodes.Status406NotAcceptable);
            request.Student.StudentCode = request.Student.StudentCode.Trim();
            request.Student.StudentName = request.Student.StudentName.Trim();
            var duplicate = await _unitOfWork.StudentRepository.GetAllNoneDeleted().Where(x => x.StudentCode.ToLower() == request.Student.StudentCode.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Student code already exists!");
            var entity = request.Student.Adapt<Student>();
            await _unitOfWork.StudentRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<StudentResponse>(), "Student " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<StudentResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
