using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Models;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Queries;

namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Handlers.QueryHandlers;

public class GetStudentFeeLedgerByIdQueryHandler : IHttpRequestHandler<GetStudentFeeLedgerByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetStudentFeeLedgerByIdQueryHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(GetStudentFeeLedgerByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty) return Result.Fail<StudentFeeLedgerResponse>(StatusCodes.Status406NotAcceptable);
            var response = await _unitOfWork.StudentFeeLedgerRepository.GetAllNoneDeleted(true).Where(x => x.Id == request.Id).Select(x => new StudentFeeLedgerResponse
            {
                Id = x.Id,
                EntryDate = x.EntryDate,
                StudentId = x.StudentId,
                StudentName = x.Student.StudentName,
                AdmissionId = x.AdmissionId,
                BranchId = x.BranchId,
                ClassId = x.ClassId,
                FinancialYearId = x.FinancialYearId,
                FinYearName = x.FinancialYear.FinYearName,
                MonthNo = x.MonthNo,
                YearNo = x.YearNo,
                FeeAmount = x.FeeAmount,
                CollectionAmount = x.CollectionAmount,
                DueAmount = x.DueAmount,
                MemoNo = x.MemoNo,
                VoucherCode = x.VoucherCode,
                Remarks = x.Remarks,
                IsCancelled = x.IsCancelled,
                IsActive = x.IsActive
            }).FirstOrDefaultAsync(cancellationToken);
            if (response is null) return Result.Fail<StudentFeeLedgerResponse>(StatusCodes.Status404NotFound);
            return Result.Success(response);
        }
        catch (Exception ex) { return Result.Fail<StudentFeeLedgerResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
