using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Branches.Models;
using SchoolManagementSystem.Application.School.Branches.Queries;

namespace SchoolManagementSystem.Application.School.Branches.Handlers.QueryHandlers;

public class GetBranchListQueryHandler : IHttpRequestHandler<GetBranchListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetBranchListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetBranchListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.BranchRepository.GetAllNoneDeleted(true);

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.BranchName.ToLower().Contains(search)
                    || (x.ContactPerson != null && x.ContactPerson.ToLower().Contains(search))
                    || (x.ContactNumber != null && x.ContactNumber.ToLower().Contains(search)));
            }

            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0)
            {
                query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            }

            var items = await query.Select(x => new BranchResponse
            {
                Id = x.Id,
                BranchName = x.BranchName,
                BranchAddress = x.BranchAddress,
                ContactPerson = x.ContactPerson,
                ContactNumber = x.ContactNumber,
                HomeThemeImagePath = x.HomeThemeImagePath,
                InstituteId = x.InstituteId!.Value,
                InstituteName = x.Institute.InstituteName,
                IsActive = x.IsActive
            }).ToListAsync(cancellationToken);

            var response = new PagedResult<BranchResponse>
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
            return Result.Fail<IList<BranchResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
