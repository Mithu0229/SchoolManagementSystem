using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Admissions.Queries;

namespace SchoolManagementSystem.Application.School.Admissions.Handlers.QueryHandlers;

public class GetAdmissionDropdownQueryHandler : IHttpRequestHandler<GetAdmissionDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAdmissionDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetAdmissionDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.AdmissionRepository.GetAllNoneDeleted(true)
                .Where(x => x.IsActive)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.Student.FullName + " - " + x.RollNo })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
