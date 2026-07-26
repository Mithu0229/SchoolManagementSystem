using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Teachers.Queries;

namespace SchoolManagementSystem.Application.School.Teachers.Handlers.QueryHandlers;

public class GetTeacherDropdownQueryHandler : IHttpRequestHandler<GetTeacherDropdownQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetTeacherDropdownQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    
    public async Task<IResult> Handle(GetTeacherDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _unitOfWork.TeacherRepository.GetAllNoneDeleted(true)
                .Select(x => new DropdownModel { Id = x.Id, Name = x.Name })
                .ToListAsync(cancellationToken);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<DropdownModel>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
