using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AcademicSessions.Queries;

namespace SchoolManagementSystem.Application.School.AcademicSessions.Handlers.QueryHandlers;

public class GetAcademicSessionDropdownQueryHandler : IHttpRequestHandler<GetAcademicSessionDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAcademicSessionDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetAcademicSessionDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.AcademicSessionRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.SessionName })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
