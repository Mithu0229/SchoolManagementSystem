using SchoolManagementSystem.Application.School.Sections.Models;
using SchoolManagementSystem.Application.School.Sections.Queries;

namespace SchoolManagementSystem.Application.School.Sections.Handlers.QueryHandlers;

public class GetSectionByIdQueryHandler : IHttpRequestHandler<GetSectionByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetSectionByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<SectionResponse>(StatusCodes.Status406NotAcceptable);
            var result = await _unitOfWork.SectionRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (result is null) return Result.Fail<SectionResponse>(StatusCodes.Status404NotFound);
            return Result.Success(result.Adapt<SectionResponse>());
        }
        catch (Exception ex) { return Result.Fail<SectionResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
