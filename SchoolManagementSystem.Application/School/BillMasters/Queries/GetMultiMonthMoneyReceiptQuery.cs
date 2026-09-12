using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.BillMasters.Models;

namespace SchoolManagementSystem.Application.School.BillMasters.Queries;

public class GetMultiMonthMoneyReceiptQuery : IHttpRequest//<MultiMonthMoneyReceiptResponse>
{
    public string VoucherNo { get; set; }
    public GetMultiMonthMoneyReceiptQuery(string voucherNo)
    {
        VoucherNo = voucherNo;
    }
}
