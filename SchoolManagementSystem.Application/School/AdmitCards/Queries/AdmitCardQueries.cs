using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.AdmitCards.Queries;

public record GetStudentAdmitCardsQuery(Guid StudentId, string? CutoffType = "exam_date") : IHttpRequest;

public record DownloadAdmitCardQuery(Guid StudentId, string ExamType, bool IgnoreDueCheck = false) : IHttpRequest;
