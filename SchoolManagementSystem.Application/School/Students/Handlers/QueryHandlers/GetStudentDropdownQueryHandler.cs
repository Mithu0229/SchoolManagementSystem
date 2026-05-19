using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Students.Queries;

namespace SchoolManagementSystem.Application.School.Students.Handlers.QueryHandlers;

public class GetStudentDropdownQueryHandler : IHttpRequestHandler<GetStudentDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetStudentDropdownQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetStudentDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var items = await _unitOfWork.StudentRepository.GetAllNoneDeleted(true)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.StudentName })
                .ToListAsync(cancellationToken);
            return Result.Success(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
