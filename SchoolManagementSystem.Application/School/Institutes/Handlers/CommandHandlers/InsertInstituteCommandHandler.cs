using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Institutes.Commands;
using SchoolManagementSystem.Application.School.Institutes.Models;

namespace SchoolManagementSystem.Application.School.Institutes.Handlers.CommandHandlers;

public class InsertInstituteCommandHandler : IHttpRequestHandler<InsertInstituteCommand>
{
    private IUnitOfWork _unitOfWork;
    private ICurrentUserService _currentUserService;
    public InsertInstituteCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IResult> Handle(InsertInstituteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return Result.Fail<InstituteResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.Institute.InstituteName = request.Institute.InstituteName.Trim();
            var institute = await _unitOfWork.InstituteRepository.GetAllNoneDeleted()
                .Where(x => x.InstituteName.ToLower() == request.Institute.InstituteName.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
            if (institute is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Institute name already exists!");
            }

            var entity = request.Institute.Adapt<Institute>();
            entity.TenantId = _currentUserService.TenantId;
            await _unitOfWork.InstituteRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var response = entity.Adapt<InstituteResponse>();
            return Result.Success(response, "Institute " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<InstituteResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
