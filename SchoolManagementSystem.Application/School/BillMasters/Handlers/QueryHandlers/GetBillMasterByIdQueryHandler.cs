using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BillMasters.Queries;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.QueryHandlers;

public class GetBillMasterByIdQueryHandler : IHttpRequestHandler<GetBillMasterByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetBillMasterByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetBillMasterByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<BillMasterResponse>(StatusCodes.Status406NotAcceptable);

            var response = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(true).Where(x => x.Id == request.Id).Select(x => new BillMasterResponse
            {
                Id = x.Id,
                AdmissionId = x.AdmissionId,
                //AdmissionRollNo = x.Admission.RollNo,
                BillMonth = x.BillMonth,
                BillYear = x.BillYear,
                TotalAmount = x.TotalAmount,
                StdCID = x.Admission.Student.StdCID,
                IsActive = x.IsActive,
                Details = x.Details.Where(d => !d.IsDeleted).Select(d => new BillDetailResponse
                {
                    Id = d.Id,
                    BillMasterId = d.BillMasterId,
                    FeeTemplateDetailId = d.FeeTemplateDetailId,
                    FeeHeadId = d.FeeHeadId,
                    //FeeHeadName = d.FeeHead.FeeHeadName,
                    Amount = d.Amount
                }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);
            if (response is null) return Result.Fail<BillMasterResponse>(StatusCodes.Status404NotFound);
            return Result.Success(response);
        }
        catch (Exception ex) { return Result.Fail<BillMasterResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
