using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.AcademicSessions.Models;
using SchoolManagementSystem.Application.School.AcademicSessions.Queries;

namespace SchoolManagementSystem.Application.School.AcademicSessions.Handlers.QueryHandlers;

public class GetAcademicSessionListQueryHandler : IHttpRequestHandler<GetAcademicSessionListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetAcademicSessionListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetAcademicSessionListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.AcademicSessionRepository.GetAllNoneDeleted(true);

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.SessionName.ToLower().Contains(search));
            }

            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0)
            {
                query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            }

            var items = await query.Select(x => new AcademicSessionResponse
            {
                Id = x.Id,
                SessionName = x.SessionName,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                IsCurrent = x.IsCurrent,
                IsActive = x.IsActive
            }).ToListAsync(cancellationToken);

            var response = new PagedResult<AcademicSessionResponse>
            {
                Items = items,
                TotalRecord = totalRecord,
                Page = pagedRequest.Page,
                PageSize = pagedRequest.PageSize
            };
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<AcademicSessionResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
