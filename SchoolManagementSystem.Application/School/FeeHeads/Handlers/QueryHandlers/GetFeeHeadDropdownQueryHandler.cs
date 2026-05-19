using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.FeeHeads.Queries;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Application.School.FeeHeads.Handlers.QueryHandlers;

public class GetFeeHeadDropdownQueryHandler : IHttpRequestHandler<GetFeeHeadDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFeeHeadDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetFeeHeadDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.FeeHeadRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.FeeHeadName })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
