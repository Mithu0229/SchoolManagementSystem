using SchoolManagementSystem.Application.School.BillMasters.Models;
using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BillMasters.Queries;

public record GetMoneyReceiptQuery(Guid Id) : IHttpRequest;
