using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeCollections.Queries;

namespace SchoolManagementSystem.Application.School.FeeCollections.Handlers.QueryHandlers;

public class GetFeeCollectionDropdownQueryHandler : IHttpRequestHandler<GetFeeCollectionDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFeeCollectionDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetFeeCollectionDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.FeeCollectionRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.MemoNo ?? x.Id.ToString() })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
