using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.BillMasters.Commands;
using SchoolManagementSystem.Application.School.BillMasters.Models;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.CommandHandlers;

public class InsertBillMasterCommandHandler : IHttpRequestHandler<InsertBillMasterCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertBillMasterCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertBillMasterCommand request, CancellationToken cancellationToken)
    {
        try
        {

            if (request is null || request.ProcessBill is null) return Result.Fail<BillMasterResponse>(StatusCodes.Status406NotAcceptable);

            var processBill = request.ProcessBill;
            processBill.AdmissionId = new Guid("22A16051-6985-4FD7-A1A9-56B0A6BE30B9");

            // 1. Validate the Admission exists and is active
            var admission = await _unitOfWork.AdmissionRepository.GetSingleNoneDeletedAsync(x => x.Id == processBill.AdmissionId);
            if (admission is null) return Result.Fail<BillMasterResponse>(StatusCodes.Status404NotFound, "Admission not found!");
            if (admission.IsCancelled) return Result.Fail<BillMasterResponse>(StatusCodes.Status400BadRequest, "Admission is cancelled!");

            // 2. Check for duplicate bill (same admission + month + year)
            var duplicate = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted()
                .Where(x => x.AdmissionId == processBill.AdmissionId && x.BillMonth == processBill.BillMonth && x.BillYear == processBill.BillYear)
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Bill already exists for this admission, month and year!");

            // 3. Find matching FeeTemplate based on Admission's ClassId, GroupId, ShiftId
            var feeTemplate = await _unitOfWork.FeeTemplateRepository.GetAllNoneDeleted(true)
                .Where(x => x.ClassId == admission.ClassId
                    //&& x.GroupId == admission.GroupId
                    && x.ShiftId == admission.ShiftId
                    && x.IsActive)
                .FirstOrDefaultAsync(cancellationToken);
            if (feeTemplate is null) return Result.Fail<BillMasterResponse>(StatusCodes.Status404NotFound, "No active Fee Template found for this admission's class, group and shift!");

            // 4. Get FeeTemplateDetails for the matched template
            var templateDetails = await _unitOfWork.FeeTemplateRepository.GetAllNoneDeleted(true)
                .Where(x => x.Id == feeTemplate.Id)
                .SelectMany(x => x.Details.Where(d => !d.IsDeleted))
                .ToListAsync(cancellationToken);
            if (templateDetails.Count == 0) return Result.Fail<BillMasterResponse>(StatusCodes.Status404NotFound, "Fee Template has no details configured!");

            // 5. Create BillMaster with TotalAmount calculated from template details
            var entity = new BillMaster
            {
                Id = Guid.NewGuid(),
                AdmissionId = processBill.AdmissionId,
                BillMonth = processBill.BillMonth,
                BillYear = processBill.BillYear,
                TotalAmount = templateDetails.Sum(x => x.Amount),
                IsActive = true,
                Details = new List<BillDetail>()
            };
            await _unitOfWork.BillMasterRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);

            // 6. Create BillDetails from each FeeTemplateDetail
            var details = templateDetails.Select(x => new BillDetail
            {
                BillMasterId = entity.Id,
                FeeTemplateDetailId = x.Id,
                FeeHeadId = x.FeeHeadId,
                Amount = x.Amount
            }).ToList();
            await _unitOfWork.BillMasterRepository.ReplaceManyAsync<BillDetail>(x => x.BillMasterId == entity.Id, details);
            await _unitOfWork.CommitAsync(cancellationToken);

            // 7. Return the created bill with details
            var response = new BillMasterResponse
            {
                Id = entity.Id,
                AdmissionId = entity.AdmissionId,
                AdmissionRollNo = admission.RollNo,
                BillMonth = entity.BillMonth,
                BillYear = entity.BillYear,
                TotalAmount = entity.TotalAmount,
                IsActive = entity.IsActive,
                Details = details.Select(d => new BillDetailResponse
                {
                    Id = d.Id,
                    BillMasterId = d.BillMasterId,
                    FeeTemplateDetailId = d.FeeTemplateDetailId,
                    FeeHeadId = d.FeeHeadId,
                    Amount = d.Amount
                }).ToList()
            };
            return Result.Success(response, "Bill processed successfully!");
        }
        catch (Exception ex) { return Result.Fail<BillMasterResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
