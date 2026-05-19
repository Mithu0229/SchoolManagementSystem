using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Branches.Queries;

namespace SchoolManagementSystem.Application.School.Branches.Handlers.QueryHandlers;

public class GetBranchDropdownQueryHandler : IHttpRequestHandler<GetBranchDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetBranchDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetBranchDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.BranchRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.BranchName })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
