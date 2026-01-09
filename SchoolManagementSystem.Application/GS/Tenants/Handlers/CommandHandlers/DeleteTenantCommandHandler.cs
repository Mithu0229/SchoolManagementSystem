using SchoolManagementSystem.Application.GS.Tenants.Commands;

namespace QSchoolManagementSystem.Application.GS.Tenants.Handlers.CommandHandlers;

public class DeleteTenantCommandHandler : IHttpRequestHandler<DeleteTenantCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteTenantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.id == Guid.Empty)
            {
                return Result.Fail<string>(StatusCodes.Status406NotAcceptable);
            }

            var enity = await _unitOfWork.TenantRepository.GetSingleNoneDeletedAsync(x => x.Id == request.id);

            if (enity == null)
            {
                return Result.Fail<string>(StatusCodes.Status404NotFound);
            }

            var result = await _unitOfWork.TenantRepository.InstantDelete(enity);

            if (result)
            {
                return Result.Success<string>("Successfully Deleted");
            }
            else
            {
                return Result.Fail<string>("Failed to Delete");
            }
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<string>(StatusCodes.Status500InternalServerError ,ex.Message);
        }
    }
}

