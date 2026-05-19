using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.StudentGroups.Queries;

namespace SchoolManagementSystem.Application.School.StudentGroups.Handlers.QueryHandlers;

public class GetStudentGroupDropdownQueryHandler : IHttpRequestHandler<GetStudentGroupDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetStudentGroupDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetStudentGroupDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.StudentGroupRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.GroupName })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
