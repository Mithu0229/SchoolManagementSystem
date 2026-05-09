using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Commands;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Models;

namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Handlers.CommandHandlers;

public class UpdateStudentFeeLedgerCommandHandler : IHttpRequestHandler<UpdateStudentFeeLedgerCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateStudentFeeLedgerCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(UpdateStudentFeeLedgerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.StudentFeeLedger.Id == Guid.Empty) return Result.Fail<StudentFeeLedgerResponse>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.StudentFeeLedgerRepository.GetSingleNoneDeletedAsync(x => x.Id == request.StudentFeeLedger.Id);
            if (entity is null) return Result.Fail<StudentFeeLedgerResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.StudentFeeLedgerRepository.GetAllNoneDeleted()
                .Where(x => x.Id != request.StudentFeeLedger.Id && x.StudentId == request.StudentFeeLedger.StudentId && x.AdmissionId == request.StudentFeeLedger.AdmissionId && x.FinancialYearId == request.StudentFeeLedger.FinancialYearId && x.MonthNo == request.StudentFeeLedger.MonthNo && x.YearNo == request.StudentFeeLedger.YearNo)
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Student fee ledger already exists!");
            entity.EntryDate = request.StudentFeeLedger.EntryDate;
            entity.StudentId = request.StudentFeeLedger.StudentId;
            entity.AdmissionId = request.StudentFeeLedger.AdmissionId;
            entity.BranchId = request.StudentFeeLedger.BranchId;
            entity.ClassId = request.StudentFeeLedger.ClassId;
            entity.FinancialYearId = request.StudentFeeLedger.FinancialYearId;
            entity.MonthNo = request.StudentFeeLedger.MonthNo;
            entity.YearNo = request.StudentFeeLedger.YearNo;
            entity.FeeAmount = request.StudentFeeLedger.FeeAmount;
            entity.CollectionAmount = request.StudentFeeLedger.CollectionAmount;
            entity.DueAmount = request.StudentFeeLedger.DueAmount;
            entity.MemoNo = request.StudentFeeLedger.MemoNo;
            entity.VoucherCode = request.StudentFeeLedger.VoucherCode;
            entity.Remarks = request.StudentFeeLedger.Remarks;
            entity.IsCancelled = request.StudentFeeLedger.IsCancelled;
            entity.IsActive = request.StudentFeeLedger.IsActive;
            await _unitOfWork.StudentFeeLedgerRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<StudentFeeLedgerResponse>(), "StudentFeeLedger " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<StudentFeeLedgerResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
