using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Sections.Queries;

namespace SchoolManagementSystem.Application.School.Sections.Handlers.QueryHandlers;

public class GetSectionDropdownQueryHandler : IHttpRequestHandler<GetSectionDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetSectionDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetSectionDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.SectionRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.SectionName })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
