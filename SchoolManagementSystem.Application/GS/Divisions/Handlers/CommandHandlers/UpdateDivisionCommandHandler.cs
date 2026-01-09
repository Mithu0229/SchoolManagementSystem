using SchoolManagementSystem.Application.GS.Divisions.Commands;
using SchoolManagementSystem.Application.GS.Divisions.Models;

namespace SchoolManagementSystem.Application.GS.Divisions.Handlers.CommandHandlers;
public class UpdateDivisionCommandHandler : IHttpRequestHandler<UpdateDivisionCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateDivisionCommandHandler(IUnitOfWork unitOfWork) 
    { 
        _unitOfWork = unitOfWork;
        
    }
    public async Task<IResult> Handle(UpdateDivisionCommand request, CancellationToken cancellationToken)
    {
       
        try
        {
            if (request is null)
            {
                return Result.Fail<DivisionResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.Division.Name = request.Division.Name.Trim();
            var entity = await _unitOfWork.DivisionRepository.GetByIdAsync((Guid)request.Division.Id,cancellationToken);

            var division = await _unitOfWork.DivisionRepository.GetByIdAsync((Guid) request.Division.Id);

            if (division != null)
            {
                return Result.Fail<DivisionResponse>(StatusCodes.Status406NotAcceptable, "Division name already exist.");
            }

            
         var result = await _unitOfWork.DivisionRepository.UpdateAsync(entity);
            if (!result)
            {
                return Result.Fail<DivisionResponse>(
                    StatusCodes.Status500InternalServerError,
                    "An unexpected issue occurred while attempting to update the division."
                );
            }
            var response = entity.Adapt<DivisionResponse>();
            return Result.Success(response, "Division " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<DivisionResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
