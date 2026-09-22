using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Application.School.BillMasters.Commands;
using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.School.BkashTransactions.Commands;
using SchoolManagementSystem.Application.School.BkashTransactions.Models;
using SchoolManagementSystem.Application.School.PayBills.Models;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.School.BkashTransactions.Handlers.CommandHandlers;

public class InsertBkashTransactionCommandHandler : IHttpRequestHandler<InsertBkashTransactionCommand>
{
    private IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;
    public InsertBkashTransactionCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration,
    IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _mediator = mediator;
    }

    public async Task<IResult> Handle(InsertBkashTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null) return Result.Fail<BkashTransactionResponse>(StatusCodes.Status406NotAcceptable);

            var req = request.BkashTransaction;

            // 1. Mandatory Field Check (Code 406)
            if (string.IsNullOrWhiteSpace(req.BillMonth) ||
                string.IsNullOrWhiteSpace(req.RefId) ||
                string.IsNullOrWhiteSpace(req.TrxId) ||
                string.IsNullOrWhiteSpace(req.Amount.ToString()))
            {
                return Result.Fail<BkashTransactionResponse>(StatusCodes.Status406NotAcceptable, "Mandatory Field missing");
            }

            // 2. Authentication Check (Code 403)

            var student = await _unitOfWork.StudentInfoRepository.GetAllNoneDeleted(false, true).FirstOrDefaultAsync(x => x.StdCID == req.RefId);
            if (student == null)
            {
                return Result.Fail<BkashTransactionResponse>(StatusCodes.Status403Forbidden, "Authentication Failed");
            }

            // 3. Parse BillMonth (MMYYYY)

            if (!TryParseBillMonth(req.BillMonth, out int month, out int year))
            {
                return Result.Fail<BkashTransactionResponse>(435, "Data Mismatch");

            }
            var currentDate = DateTime.Now;

            var billDate = new DateTime(year, month, 1);
            var currentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            if (billDate > currentMonth)
            {
                return Result.Fail<BkashTransactionResponse>(437, "Due date over");
            }

            //var billMaster = await _unitOfWork.BillMasterRepository
            //            .GetAllNoneDeleted(false, true)
            //            .Include(x => x.Admission)
            //                .ThenInclude(x => x.Student)
            //            .Include(x => x.Details)
            //            .Where(x =>
            //                (x.BillYear < year ||
            //                (x.BillYear == year && x.BillMonth <= month))
            //                && !x.IsPaid)
            //            .OrderBy(x => x.BillYear)
            //            .ThenBy(x => x.BillMonth)
            //            .ToListAsync(cancellationToken);

            var billMaster = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false, true)
                .Include(x => x.Admission)
                    .ThenInclude(a => a.Student)
                .Include(x => x.Details)
                    .ThenInclude(d => d.FeeHead)
                .Where(x => (x.BillYear < year ||
                            (x.BillYear == year && x.BillMonth <= month))
                            && !x.IsPaid && !x.IsActive &&
                            ((x.Admission != null && x.Admission.Student != null && x.Admission.Student.StdCID!.ToLower() == req.RefId)))
                .ToListAsync(cancellationToken);
            if (billMaster.Count == 0)
            {
                var Paidbill = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false, true)
                .Include(x => x.Admission)
                    .ThenInclude(a => a.Student)
                .Include(x => x.Details)
                    .ThenInclude(d => d.FeeHead)
                .Where(x => (x.BillYear < year ||
                            (x.BillYear == year && x.BillMonth <= month))
                            && x.IsPaid && x.IsActive &&
                            ((x.Admission != null && x.Admission.Student != null && x.Admission.Student.StdCID!.ToLower() == req.RefId)))
                .ToListAsync(cancellationToken);
                if (Paidbill.Count > 0)
                {
                    return Result.Fail<BkashTransactionResponse>(436, "Already paid");
                }
                return Result.Fail<BkashTransactionResponse>(404, "Data not found");

            }

            var unpaidBillMasterIds = billMaster
                .Where(x => x.Admission.Student.StdCID == req.RefId)
                .Select(x => x.Id)
                .ToList();

            MultiMonthBillCollectionRequest requestMul = new MultiMonthBillCollectionRequest
            {
                BillMonth = month,
                BillYear = year,
                CollectionAmount = req.Amount,
                BillMasterIds = unpaidBillMasterIds,
                TransactionType = TransactionType.Bkash,
                TrxId = req.TrxId

            };

            //     var entity = await _mediator.Send(
            //            new MultiMonthBillCollectionCommand()
            //            {
            //                Request = requestMul
            //            },
            //cancellationToken);

            //     return Result.Success(entity, "BkashTransaction " + AlertMessage.SaveMessage);

            var result = await _mediator.Send(
                        new MultiMonthBillCollectionCommand
                        {
                            Request = requestMul
                        },
                        cancellationToken);

            if (!result.IsSuccess)
            {
                return result;
            }

            var collectionResult = result as IResult<MultiMonthBillCollectionResponse>;
            var collectionResponse = collectionResult?.Data;

            if (collectionResponse == null)
            {
                return Result.Fail<BkashTransactionResponse>(
                    StatusCodes.Status500InternalServerError,
                    "Invalid bill collection response");
            }

            var response = new BkashTransactionResponse
            {
                TrxId = collectionResponse.TrxId,
                TotalAmount = collectionResponse.TotalAmount.ToString(),
                PaidAmount = collectionResponse.PaidAmount.ToString(),
                DueAmount = collectionResponse.DueAmount.ToString(),
                //ErrorMsg = collectionResponse.Message!,
                ConsumerName = collectionResponse.StCID

            };

            return Result.Success(
                response,
                collectionResponse.Message!,200);
        }
        catch (Exception ex)
        {
            return Result.Fail<BkashTransactionResponse>(435, $"Data Mismatch: {ex.Message}");
        }
    }

    private bool TryParseBillMonth(string billMonth, out int month, out int year)
    {
        month = 0;
        year = 0;
        if (string.IsNullOrWhiteSpace(billMonth) || billMonth.Length != 6) return false;

        if (int.TryParse(billMonth.Substring(0, 2), out month) &&
            int.TryParse(billMonth.Substring(2, 4), out year))
        {
            return month >= 1 && month <= 12 && year >= 2000 && year <= 2100;
        }

        return false;
    }
}



