using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.StudentGroups.Commands;
using SchoolManagementSystem.Application.School.StudentGroups.Models;

namespace SchoolManagementSystem.Application.School.StudentGroups.Handlers.CommandHandlers;

public class UpdateStudentGroupCommandHandler : IHttpRequestHandler<UpdateStudentGroupCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateStudentGroupCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateStudentGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.StudentGroup.Id == Guid.Empty) return Result.Fail<StudentGroupResponse>(StatusCodes.Status406NotAcceptable);
            request.StudentGroup.GroupName = request.StudentGroup.GroupName.Trim();
            var entity = await _unitOfWork.StudentGroupRepository.GetSingleNoneDeletedAsync(x => x.Id == request.StudentGroup.Id);
            if (entity is null) return Result.Fail<StudentGroupResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.StudentGroupRepository.GetAllNoneDeleted().Where(x => x.Id != request.StudentGroup.Id && x.GroupName.ToLower() == request.StudentGroup.GroupName.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Student group already exists!");
            entity.GroupName = request.StudentGroup.GroupName;
            entity.GroupDetails = request.StudentGroup.GroupDetails;
            entity.IsActive = request.StudentGroup.IsActive;
            await _unitOfWork.StudentGroupRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<StudentGroupResponse>(), "StudentGroup " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<StudentGroupResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
