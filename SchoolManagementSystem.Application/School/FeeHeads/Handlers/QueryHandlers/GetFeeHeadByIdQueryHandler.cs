using SchoolManagementSystem.Application.School.FeeHeads.Models;
using SchoolManagementSystem.Application.School.FeeHeads.Queries;

namespace SchoolManagementSystem.Application.School.FeeHeads.Handlers.QueryHandlers;

public class GetFeeHeadByIdQueryHandler : IHttpRequestHandler<GetFeeHeadByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFeeHeadByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetFeeHeadByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<FeeHeadResponse>(StatusCodes.Status406NotAcceptable);
            var result = await _unitOfWork.FeeHeadRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (result is null) return Result.Fail<FeeHeadResponse>(StatusCodes.Status404NotFound);
            return Result.Success(result.Adapt<FeeHeadResponse>());
        }
        catch (Exception ex) { return Result.Fail<FeeHeadResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
