using SchoolManagementSystem.Application.School.Admissions.Models;

namespace SchoolManagementSystem.Application.School.Admissions.Commands;

public class InsertAdmissionCommand : IHttpRequest
{
    public AdmissionRequest Admission { get; set; }
}
