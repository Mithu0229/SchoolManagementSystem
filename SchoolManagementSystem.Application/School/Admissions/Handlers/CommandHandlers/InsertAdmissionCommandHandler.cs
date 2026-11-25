using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Admissions.Commands;
using SchoolManagementSystem.Application.School.Admissions.Models;
using SchoolManagementSystem.Application.School.BillMasters.Models;

namespace SchoolManagementSystem.Application.School.Admissions.Handlers.CommandHandlers;

public class InsertAdmissionCommandHandler : IHttpRequestHandler<InsertAdmissionCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertAdmissionCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertAdmissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.AdmissionRepository.GetAllNoneDeleted(false, true)
                .Where(x => x.BranchId == request.Admission.BranchId && x.StudentId == request.Admission.StudentId)
                .FirstOrDefaultAsync(cancellationToken);
            if(student != null)
            {
                return Result.Fail<AdmissionResponse>(StatusCodes.Status403Forbidden, "Admission already exists");
            }

            var entity = request.Admission.Adapt<Admission>();
            entity.Id = Guid.NewGuid(); 
            await _unitOfWork.AdmissionRepository.AddAsync(entity);
            
            if (request.Admission.MonthlyFeeAmount > 0)
            {
                var accademicSession = await _unitOfWork.AcademicSessionRepository.GetAllNoneDeleted(false, true)
                    .Where(x => x.Id == request.Admission.AcademicSessionId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (accademicSession == null)
                {
                    return Result.Fail<AdmissionResponse>(StatusCodes.Status404NotFound, "Academic session not found");
                }

                // entry bill for every month from academic session FromDate to ToDate
                var now = DateTime.Now;
                var monthCursor = new DateTime(accademicSession.FromDate.Year, accademicSession.FromDate.Month, 1);
                var lastMonth = new DateTime(accademicSession.ToDate.Year, accademicSession.ToDate.Month, 1);
                int voucherSeq = 0;

                for (; monthCursor <= lastMonth; monthCursor = monthCursor.AddMonths(1))
                {
                    voucherSeq++;
                    var bill = new BillMaster
                    {
                        Id = Guid.NewGuid(),
                        AdmissionId = entity.Id,
                        BillMonth = monthCursor.Month,
                        BillYear = monthCursor.Year,
                        TotalAmount = request.Admission.MonthlyFeeAmount,
                        IsActive = false,
                        Details = new List<BillDetail>(),
                        VoucherNo = voucherSeq.ToString(),
                        CreatedDate = new DateTime(monthCursor.Year, monthCursor.Month, 1, now.Hour, now.Minute, now.Second)
                    };


                    // 6. Create BillDetails from each bill master

                    var billDetail = new BillDetail
                    {
                        Id = Guid.NewGuid(),
                        BillMasterId = bill.Id,
                        FeeTemplateDetailId = Guid.Empty,
                        FeeHeadId = Guid.Parse("28A6E032-647D-40DC-B31D-0754FDE086E8"),//monthly fee head
                        Amount = request.Admission.MonthlyFeeAmount,
                        
                    };
                    bill.Details.Add(billDetail);
                    await _unitOfWork.BillMasterRepository.AddAsync(bill);
                }
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<AdmissionResponse>(), "Admission " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            var message = ex.InnerException?.Message ?? ex.Message;

            return Result.Fail<AdmissionResponse>(
                StatusCodes.Status500InternalServerError,
                $"Error: {message}"
            );
        }
    }
}
