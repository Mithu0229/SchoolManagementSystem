using SchoolManagementSystem.Application.School.StudentFeeLedgers.Models;

namespace SchoolManagementSystem.Application.School.StudentFeeLedgers.Commands;

public class UpdateStudentFeeLedgerCommand : IHttpRequest
{
    public StudentFeeLedgerRequest StudentFeeLedger { get; set; }
}
