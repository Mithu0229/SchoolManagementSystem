using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Institutes.Commands;
using SchoolManagementSystem.Application.School.Institutes.Models;

namespace SchoolManagementSystem.Application.School.Institutes.Handlers.CommandHandlers;

public class UpdateInstituteCommandHandler : IHttpRequestHandler<UpdateInstituteCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateInstituteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(UpdateInstituteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.Institute.Id == Guid.Empty)
            {
                return Result.Fail<InstituteResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.Institute.InstituteName = request.Institute.InstituteName.Trim();
            var entity = await _unitOfWork.InstituteRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Institute.Id);
            if (entity is null)
            {
                return Result.Fail<InstituteResponse>(StatusCodes.Status404NotFound);
            }

            var duplicate = await _unitOfWork.InstituteRepository.GetAllNoneDeleted()
                .Where(x => x.Id != request.Institute.Id && x.InstituteName.ToLower() == request.Institute.InstituteName.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Institute name already exists!");
            }

            entity.InstituteName = request.Institute.InstituteName;
            entity.Address = request.Institute.Address;
            entity.ContactNo = request.Institute.ContactNo;
            entity.Email = request.Institute.Email;
            entity.LogoPath = request.Institute.LogoPath;
            entity.IsActive = request.Institute.IsActive;

            await _unitOfWork.InstituteRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var response = entity.Adapt<InstituteResponse>();
            return Result.Success(response, "Institute " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<InstituteResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
