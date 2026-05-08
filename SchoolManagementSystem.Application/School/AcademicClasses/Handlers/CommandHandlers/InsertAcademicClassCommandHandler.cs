using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AcademicClasses.Commands;
using SchoolManagementSystem.Application.School.AcademicClasses.Models;

namespace SchoolManagementSystem.Application.School.AcademicClasses.Handlers.CommandHandlers;

public class InsertAcademicClassCommandHandler : IHttpRequestHandler<InsertAcademicClassCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertAcademicClassCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(InsertAcademicClassCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return Result.Fail<AcademicClassResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.AcademicClass.ClassName = request.AcademicClass.ClassName.Trim();
            var duplicate = await _unitOfWork.AcademicClassRepository.GetAllNoneDeleted().Where(x => x.ClassName.ToLower() == request.AcademicClass.ClassName.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Academic class already exists!");
            }

            var entity = request.AcademicClass.Adapt<AcademicClass>();
            await _unitOfWork.AcademicClassRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var response = entity.Adapt<AcademicClassResponse>();
            return Result.Success(response, "AcademicClass " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<AcademicClassResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
