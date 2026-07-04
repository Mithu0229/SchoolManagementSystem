using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Application.School.BillMasters.Commands;
using SchoolManagementSystem.Application.School.BillMasters.Models;

namespace SchoolManagementSystem.Application.School.BillMasters.Handlers.CommandHandlers;

public class UpdateBillMasterCommandHandler : IHttpRequestHandler<UpdateBillMasterCommand>
{
    private IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public UpdateBillMasterCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration) 
    { 
        _unitOfWork = unitOfWork; 
        _configuration = configuration;
    }
    public async Task<IResult> Handle(UpdateBillMasterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.BillMaster.Id == Guid.Empty) return Result.Fail<BillMasterResponse>(StatusCodes.Status406NotAcceptable);
            var entity = await _unitOfWork.BillMasterRepository.GetSingleNoneDeletedAsync(x => x.Id == request.BillMaster.Id);
            if (entity is null) return Result.Fail<BillMasterResponse>(StatusCodes.Status404NotFound);
            var duplicate = await _unitOfWork.BillMasterRepository.GetAllNoneDeleted()
                .Where(x => x.Id != request.BillMaster.Id && x.AdmissionId == request.BillMaster.AdmissionId && x.BillMonth == request.BillMaster.BillMonth && x.BillYear == request.BillMaster.BillYear)
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null) return Result.Fail(StatusCodes.Status409Conflict, "Bill already exists for this admission, month and year!");
            entity.AdmissionId = request.BillMaster.AdmissionId;
            entity.BillMonth = request.BillMaster.BillMonth;
            entity.BillYear = request.BillMaster.BillYear;
            entity.TotalAmount = request.BillMaster.TotalAmount;
            entity.IsActive = true;
            await _unitOfWork.BillMasterRepository.UpdateAsync(entity);
            var details = request.BillMaster.Details.Select(x => new BillDetail
            {
                BillMasterId = entity.Id,
                FeeTemplateDetailId = x.FeeTemplateDetailId,
                FeeHeadId = x.FeeHeadId,
                Amount = x.Amount
            }).ToList();
            await _unitOfWork.BillMasterRepository.ReplaceManyAsync<BillDetail>(x => x.BillMasterId == entity.Id, details);
            await _unitOfWork.CommitAsync(cancellationToken);
            //send sms
            try
            {
                string studentName = request.BillMaster.StdCID!;
                decimal paidAmount = request.BillMaster.TotalAmount;

                string message = $"The bill for student {studentName} has been paid successfully. Paid Amount: ৳{paidAmount:N2}.";

                var smsPayload = new
                {
                    apikey = _configuration["SmsSettings:ApiKey"],
                    secretkey = _configuration["SmsSettings:SecretKey"],
                    callerID = _configuration["SmsSettings:CallerID"],
                    toUser = "8801755948794",
                    messageContent = message
                };

                using var httpClient = new System.Net.Http.HttpClient();
                var content = new System.Net.Http.StringContent(System.Text.Json.JsonSerializer.Serialize(smsPayload), System.Text.Encoding.UTF8, "application/json");
                // Note: Ensure the endpoint path (e.g. /api/v1/send or /smsapi) matches what Songbird Telecom expects
                var response = await httpClient.PostAsync("http://sms.songbirdtelecom.com:8746/sendtext", content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMS Error: {ex.Message}");
            }

            return Result.Success(entity.Adapt<BillMasterResponse>(), "BillMaster " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex) { return Result.Fail<BillMasterResponse>(StatusCodes.Status500InternalServerError, ex.Message); }
    }
}
