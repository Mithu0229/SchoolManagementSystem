using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.School.PayBills.Commands;
using SchoolManagementSystem.Application.School.PayBills.Models;
using SchoolManagementSystem.Application.School.PayBills.Queries;
using System.Text.Json;

namespace SchoolManagementSystem.API.Controllers;

[ApiController]
public class PayBillController : PublicBaseController
{
    /// <summary>
    /// Check Bill Endpoint (Outbound Interface)
    /// PDF Spec: 1.1.1 Check Bill (POST /api/queryBill)
    /// </summary>
    [HttpPost("/api/queryBill")]
    [HttpPost("check-bill")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CheckBillResponse))]
    public async Task<IActionResult> CheckBill()
    {
        var requestModel = await ExtractRequestDataAsync<CheckBillRequest>();
        var result = await Mediator.Send(new CheckBillQuery { Request = requestModel });
        return Ok(result);
    }

    /// <summary>
    /// Bill Payment Endpoint (Call-back Request Schema)
    /// PDF Spec: 1.2.1 Bill Payment (POST /api/payBill)
    /// </summary>
    [HttpPost("/api/payBill")]
    [HttpPost("pay-bill")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillPaymentResponse))]
    public async Task<IActionResult> BillPayment()
    {
        var requestModel = await ExtractRequestDataAsync<BillPaymentRequest>();
        var result = await Mediator.Send(new BillPaymentCommand { Request = requestModel });
        return Ok(result);
    }

    /// <summary>
    /// Transaction Search Query Endpoint (TSQ)
    /// PDF Spec: 1.3.1 TSQ (POST /api/searchTransaction)
    /// </summary>
    [HttpPost("/api/searchTransaction")]
    [HttpPost("tsq")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TSQResponse))]
    public async Task<IActionResult> TSQ()
    {
        var requestModel = await ExtractRequestDataAsync<TSQRequest>();
        var result = await Mediator.Send(new TSQQuery { Request = requestModel });
        return Ok(result);
    }

    private async Task<T> ExtractRequestDataAsync<T>() where T : new()
    {
        var model = new T();

        // 1. Try reading JSON body if present
        if (Request.HasJsonContentType() || (Request.ContentType != null && Request.ContentType.Contains("application/json")))
        {
            try
            {
                Request.EnableBuffering();
                Request.Body.Position = 0;
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var deserialized = await JsonSerializer.DeserializeAsync<T>(Request.Body, jsonOptions);
                if (deserialized != null)
                {
                    model = deserialized;
                }
            }
            catch
            {
                // Fallback to form/query parsing
            }
        }

        // 2. Read Form parameters if present (multipart/form-data or application/x-www-form-urlencoded)
        if (Request.HasFormContentType && Request.Form != null)
        {
            SetProperty(model, nameof(CheckBillRequest.UserName), Request.Form["UserName"].FirstOrDefault());
            SetProperty(model, nameof(CheckBillRequest.Password), Request.Form["Password"].FirstOrDefault());
            SetProperty(model, nameof(CheckBillRequest.CustomerNo), Request.Form["CustomerNo"].FirstOrDefault());
            SetProperty(model, nameof(CheckBillRequest.AccNo), Request.Form["AccNo"].FirstOrDefault());
            SetProperty(model, nameof(CheckBillRequest.MeterNo), Request.Form["MeterNo"].FirstOrDefault());
            SetProperty(model, nameof(CheckBillRequest.BillNo), Request.Form["BillNo"].FirstOrDefault());
            SetProperty(model, nameof(CheckBillRequest.RefID), Request.Form["RefID"].FirstOrDefault());
            SetProperty(model, nameof(CheckBillRequest.BillMonth), Request.Form["BillMonth"].FirstOrDefault());
            SetProperty(model, nameof(CheckBillRequest.Amount), Request.Form["Amount"].FirstOrDefault());
            SetProperty(model, nameof(BillPaymentRequest.UserMobileNumber), Request.Form["UserMobileNumber"].FirstOrDefault());
            SetProperty(model, nameof(BillPaymentRequest.TrxId), Request.Form["TrxId"].FirstOrDefault());
            SetProperty(model, nameof(BillPaymentRequest.PayTime), Request.Form["PayTime"].FirstOrDefault());
        }

        // 3. Fallback to Query string parameters if fields are still empty
        if (Request.Query != null && Request.Query.Count > 0)
        {
            SetPropertyIfEmpty(model, nameof(CheckBillRequest.UserName), Request.Query["UserName"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(CheckBillRequest.Password), Request.Query["Password"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(CheckBillRequest.CustomerNo), Request.Query["CustomerNo"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(CheckBillRequest.AccNo), Request.Query["AccNo"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(CheckBillRequest.MeterNo), Request.Query["MeterNo"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(CheckBillRequest.BillNo), Request.Query["BillNo"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(CheckBillRequest.RefID), Request.Query["RefID"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(CheckBillRequest.BillMonth), Request.Query["BillMonth"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(CheckBillRequest.Amount), Request.Query["Amount"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(BillPaymentRequest.UserMobileNumber), Request.Query["UserMobileNumber"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(BillPaymentRequest.TrxId), Request.Query["TrxId"].FirstOrDefault());
            SetPropertyIfEmpty(model, nameof(BillPaymentRequest.PayTime), Request.Query["PayTime"].FirstOrDefault());
        }

        return model;
    }

    private void SetProperty<T>(T target, string propertyName, string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        var prop = typeof(T).GetProperty(propertyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
        if (prop != null && prop.CanWrite && prop.PropertyType == typeof(string))
        {
            prop.SetValue(target, value.Trim());
        }
    }

    private void SetPropertyIfEmpty<T>(T target, string propertyName, string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        var prop = typeof(T).GetProperty(propertyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
        if (prop != null && prop.CanWrite && prop.PropertyType == typeof(string))
        {
            var existingValue = prop.GetValue(target) as string;
            if (string.IsNullOrWhiteSpace(existingValue))
            {
                prop.SetValue(target, value.Trim());
            }
        }
    }
}
