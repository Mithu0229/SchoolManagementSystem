using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.StudentGroups.Commands;
using SchoolManagementSystem.Application.School.StudentGroups.Models;

namespace SchoolManagementSystem.Application.School.StudentGroups.Handlers.CommandHandlers;

public class InsertStudentGroupCommandHandler : IHttpRequestHandler<InsertStudentGroupCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertStudentGroupCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertStudentGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<StudentGroupResponse>(StatusCodes.Status406NotAcceptable);
            request.StudentGroup.GroupName = request.StudentGroup.GroupName.Trim();
            var duplicate = await _unitOfWork.StudentGroupRepository.GetAllNoneDeleted().Where(x => x.GroupName.ToLower() == request.StudentGroup.GroupName.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Student group already exists!");
            var entity = request.StudentGroup.Adapt<StudentGroup>();
            await _unitOfWork.StudentGroupRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<StudentGroupResponse>(), "StudentGroup " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<StudentGroupResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
