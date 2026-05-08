using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Institutes.Models;
using SchoolManagementSystem.Application.School.Institutes.Queries;

namespace SchoolManagementSystem.Application.School.Institutes.Handlers.QueryHandlers;

public class GetInstituteListQueryHandler : IHttpRequestHandler<GetInstituteListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetInstituteListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetInstituteListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.InstituteRepository.GetAllNoneDeleted(true);

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.InstituteName.ToLower().Contains(search)
                    || (x.ContactNo != null && x.ContactNo.ToLower().Contains(search))
                    || (x.Email != null && x.Email.ToLower().Contains(search)));
            }

            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0)
            {
                query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            }

            var items = await query.Select(x => new InstituteResponse
            {
                Id = x.Id,
                InstituteName = x.InstituteName,
                Address = x.Address,
                ContactNo = x.ContactNo,
                Email = x.Email,
                LogoPath = x.LogoPath,
                IsActive = x.IsActive
            }).ToListAsync(cancellationToken);

            var response = new PagedResult<InstituteResponse>
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
            return Result.Fail<IList<InstituteResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
