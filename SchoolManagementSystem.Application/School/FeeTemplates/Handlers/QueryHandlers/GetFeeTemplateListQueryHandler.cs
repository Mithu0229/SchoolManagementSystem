using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeTemplates.Models;
using SchoolManagementSystem.Application.School.FeeTemplates.Queries;

namespace SchoolManagementSystem.Application.School.FeeTemplates.Handlers.QueryHandlers;

public class GetFeeTemplateListQueryHandler : IHttpRequestHandler<GetFeeTemplateListQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFeeTemplateListQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetFeeTemplateListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.FeeTemplateRepository.GetAllNoneDeleted(true);
            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.TemplateName.ToLower().Contains(search) || x.Class.ClassName.ToLower().Contains(search));
            }
            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0) query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            var items = await query.Select(x => new FeeTemplateResponse
            {
                Id = x.Id,
                TemplateName = x.TemplateName,
                ClassId = x.ClassId,
                ClassName = x.Class.ClassName,
                GroupId = x.GroupId,
                GroupName = x.Group == null ? null : x.Group.GroupName,
                ShiftId = x.ShiftId,
                ShiftName = x.Shift == null ? null : x.Shift.ShiftName,
                IsActive = x.IsActive
            }).ToListAsync(cancellationToken);
            return Result.Success(new PagedResult<FeeTemplateResponse> { Items = items, TotalRecord = totalRecord, Page = pagedRequest.Page, PageSize = pagedRequest.PageSize });
        }
        catch (Exception ex) { return Result.Fail<IList<FeeTemplateResponse>>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
