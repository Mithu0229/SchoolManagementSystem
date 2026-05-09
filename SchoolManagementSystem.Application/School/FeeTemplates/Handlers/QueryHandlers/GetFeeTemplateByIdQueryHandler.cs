using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.FeeTemplates.Models;
using SchoolManagementSystem.Application.School.FeeTemplates.Queries;

namespace SchoolManagementSystem.Application.School.FeeTemplates.Handlers.QueryHandlers;

public class GetFeeTemplateByIdQueryHandler : IHttpRequestHandler<GetFeeTemplateByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetFeeTemplateByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetFeeTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<FeeTemplateResponse>(StatusCodes.Status406NotAcceptable);
            var response = await _unitOfWork.FeeTemplateRepository.GetAllNoneDeleted(true).Where(x => x.Id == request.Id).Select(x => new FeeTemplateResponse
            {
                Id = x.Id,
                TemplateName = x.TemplateName,
                ClassId = x.ClassId,
                ClassName = x.Class.ClassName,
                GroupId = x.GroupId,
                GroupName = x.Group == null ? null : x.Group.GroupName,
                ShiftId = x.ShiftId,
                ShiftName = x.Shift == null ? null : x.Shift.ShiftName,
                IsActive = x.IsActive,
                Details = x.Details.Where(d => !d.IsDeleted).Select(d => new FeeTemplateDetailResponse { Id = d.Id, FeeTemplateId = d.FeeTemplateId, FeeHeadId = d.FeeHeadId, FeeHeadName = d.FeeHead.FeeHeadName, Amount = d.Amount }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);
            if (response is null) return Result.Fail<FeeTemplateResponse>(StatusCodes.Status404NotFound);
            return Result.Success(response);
        }
        catch (Exception ex) { return Result.Fail<FeeTemplateResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
