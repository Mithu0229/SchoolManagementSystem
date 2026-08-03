using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AcademicClasses.Queries;

namespace SchoolManagementSystem.Application.School.AcademicClasses.Handlers.QueryHandlers;

public class GetAcademicClassDropdownQueryHandler : IHttpRequestHandler<GetAcademicClassDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAcademicClassDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetAcademicClassDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.AcademicClassRepository.GetAllNoneDeleted(false,true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.ClassName })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
