using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AcademicClasses.Models;
using SchoolManagementSystem.Application.School.AcademicClasses.Queries;

namespace SchoolManagementSystem.Application.School.AcademicClasses.Handlers.QueryHandlers;

public class GetAcademicClassListQueryHandler : IHttpRequestHandler<GetAcademicClassListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAcademicClassListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetAcademicClassListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.AcademicClassRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.ClassName.ToLower().Contains(search) || (x.ClassDetails != null && x.ClassDetails.ToLower().Contains(search)));
            }

            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0)
            {
                query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            }

            var items = await query.Select(x => new AcademicClassResponse { Id = x.Id, ClassName = x.ClassName, ClassDetails = x.ClassDetails, IsActive = x.IsActive }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<AcademicClassResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<AcademicClassResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
