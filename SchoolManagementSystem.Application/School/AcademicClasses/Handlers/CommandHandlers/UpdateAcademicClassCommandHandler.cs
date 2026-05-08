using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AcademicClasses.Commands;
using SchoolManagementSystem.Application.School.AcademicClasses.Models;

namespace SchoolManagementSystem.Application.School.AcademicClasses.Handlers.CommandHandlers;

public class UpdateAcademicClassCommandHandler : IHttpRequestHandler<UpdateAcademicClassCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateAcademicClassCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(UpdateAcademicClassCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.AcademicClass.Id == Guid.Empty)
            {
                return Result.Fail<AcademicClassResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.AcademicClass.ClassName = request.AcademicClass.ClassName.Trim();
            var entity = await _unitOfWork.AcademicClassRepository.GetSingleNoneDeletedAsync(x => x.Id == request.AcademicClass.Id);
            if (entity is null)
            {
                return Result.Fail<AcademicClassResponse>(StatusCodes.Status404NotFound);
            }

            var duplicate = await _unitOfWork.AcademicClassRepository.GetAllNoneDeleted().Where(x => x.Id != request.AcademicClass.Id && x.ClassName.ToLower() == request.AcademicClass.ClassName.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Academic class already exists!");
            }

            entity.ClassName = request.AcademicClass.ClassName;
            entity.ClassDetails = request.AcademicClass.ClassDetails;
            entity.IsActive = request.AcademicClass.IsActive;
            await _unitOfWork.AcademicClassRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var response = entity.Adapt<AcademicClassResponse>();
            return Result.Success(response, "AcademicClass " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<AcademicClassResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
