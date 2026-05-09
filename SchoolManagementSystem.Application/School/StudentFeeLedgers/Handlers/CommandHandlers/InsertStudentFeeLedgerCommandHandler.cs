using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Commands;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Models;

namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Handlers.CommandHandlers;

public class InsertStudentFeeLedgerCommandHandler : IHttpRequestHandler<InsertStudentFeeLedgerCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertStudentFeeLedgerCommandHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IResult> Handle(InsertStudentFeeLedgerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<StudentFeeLedgerResponse>(StatusCodes.Status406NotAcceptable);
            var duplicate = await _unitOfWork.StudentFeeLedgerRepository.GetAllNoneDeleted()
                .Where(x => x.StudentId == request.StudentFeeLedger.StudentId && x.AdmissionId == request.StudentFeeLedger.AdmissionId && x.FinancialYearId == request.StudentFeeLedger.FinancialYearId && x.MonthNo == request.StudentFeeLedger.MonthNo && x.YearNo == request.StudentFeeLedger.YearNo)
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Student fee ledger already exists!");
            var entity = request.StudentFeeLedger.Adapt<StudentFeeLedger>();
            await _unitOfWork.StudentFeeLedgerRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<StudentFeeLedgerResponse>(), "StudentFeeLedger " + AlertMessage.SaveMessage);
        }
        catch (Exception ex) { return Result.Fail<StudentFeeLedgerResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
