using SchoolManagementSystem.Application.GS.Divisions.Commands;
using SchoolManagementSystem.Application.GS.Divisions.Models;

namespace SchoolManagementSystem.Application.GS.Divisions.Handlers.CommandHandlers;
public class InsertDivisionCommandHandler : IHttpRequestHandler<InsertDivisionCommand>
{
    private IUnitOfWork _unitOfWork;
   
    public InsertDivisionCommandHandler(IUnitOfWork unitOfWork)
    { 
        _unitOfWork = unitOfWork;
    }
    public async Task<IResult> Handle(InsertDivisionCommand request, CancellationToken cancellationToken)
    {
        
        try
        {
            if (request is null)
            {
                return Result.Fail<DivisionResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.Division.Name = request.Division.Name.Trim();
            var id = request.Division.Id;

            var division = await _unitOfWork.DivisionRepository.GetByIdAsync((Guid)id,cancellationToken);
               // Using Result to avoid async/await in the handler
            
            if (division is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "division name already exists!");
            }

            var entity = request.Division.Adapt<Division>();
            await _unitOfWork.DivisionRepository.AddAsync(entity);
            var result = await _unitOfWork.CommitAsync();
            var response = entity.Adapt<DivisionResponse>();
            return Result.Success(response, "Division " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex);
            return Result.Fail<DivisionResponse>(StatusCodes.Status500InternalServerError);
        }
    }
}



