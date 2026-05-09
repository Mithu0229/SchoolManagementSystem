using SchoolManagementSystem.Application.School.StudentFeeLedgers.Models;

namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Commands;

public class InsertStudentFeeLedgerCommand : IHttpRequest
{
    public StudentFeeLedgerRequest StudentFeeLedger { get; set; }
}
