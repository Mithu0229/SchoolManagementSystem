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

            if(request.Admission.MonthlyFeeAmount > 0)
            {
                //entry bill for 12 month

                for (int i = 1; i <= 12; i++)
                {
                    var bill = new BillMaster
                    {
                        Id = Guid.NewGuid(),
                        AdmissionId = entity.Id,
                        BillMonth = i,
                        BillYear = DateTime.Now.Year,
                        TotalAmount = request.Admission.MonthlyFeeAmount,
                        IsActive = false,
                        Details = new List<BillDetail>()
                    };


                    // 6. Create BillDetails from each bill master

                    var billDetail = new BillDetail
                    {
                        Id = Guid.NewGuid(),
                        BillMasterId = bill.Id,
                        FeeTemplateDetailId = Guid.Empty,
                        FeeHeadId = Guid.Empty,
                        Amount = request.Admission.MonthlyFeeAmount
                    };
                    bill.Details.Add(billDetail);
                    await _unitOfWork.BillMasterRepository.AddAsync(bill);
                }
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<AdmissionResponse>(), "Admission " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<AdmissionResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
