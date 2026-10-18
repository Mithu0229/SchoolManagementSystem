using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.ExamMarks.Commands;
using SchoolManagementSystem.Application.School.ExamMarks.Models;
using SchoolManagementSystem.Application.School.ExamMarks.Queries;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Application.School.ExamMarks.Handlers;

public class ExamMarkHandlers :
    IHttpRequestHandler<SaveStudentMarksCommand>,
    IHttpRequestHandler<SaveBatchMarksCommand>,
    IHttpRequestHandler<SaveExamComponentCommand>,
    IHttpRequestHandler<GetExamComponentsQuery>,
    IHttpRequestHandler<GetStudentMarksheetQuery>,
    IHttpRequestHandler<GetBatchMarksQuery>,
    IHttpRequestHandler<GetStudentsByClassQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public ExamMarkHandlers(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(SaveStudentMarksCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var req = request.Request;
            var session = req.AcademicSessionId;
            var studentId = req.StudentId;
            var classId = req.ClassId;
            var sectionId = req.SectionId;

            // Load existing marks for this student and session
            var existingMarks = await _unitOfWork.StudentExamMarkRepository.GetAllNoneDeleted()
                .Where(x => x.AcademicSessionId == session && x.StudentId == studentId)
                .ToListAsync(cancellationToken);

            foreach (var subjectRow in req.SubjectMarks)
            {
                var subjectId = subjectRow.SubjectId;
                foreach (var compMark in subjectRow.ComponentMarks)
                {
                    var existing = existingMarks.FirstOrDefault(x => x.SubjectId == subjectId && x.ExamComponentId == compMark.ExamComponentId);
                    if (existing != null)
                    {
                        existing.MarksObtained = compMark.MarksObtained;
                        existing.IsAbsent = compMark.IsAbsent;
                        await _unitOfWork.StudentExamMarkRepository.UpdateAsync(existing);
                    }
                    else
                    {
                        var newMark = new StudentExamMark
                        {
                            AcademicSessionId = session,
                            ClassId = classId,
                            SectionId = sectionId,
                            StudentId = studentId,
                            SubjectId = subjectId,
                            ExamComponentId = compMark.ExamComponentId,
                            MarksObtained = compMark.MarksObtained,
                            IsAbsent = compMark.IsAbsent,
                            IsActive = true
                        };
                        await _unitOfWork.StudentExamMarkRepository.AddAsync(newMark);
                    }
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(true, "Student marks saved successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(SaveBatchMarksCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var req = request.Request;
            var existingMarks = await _unitOfWork.StudentExamMarkRepository.GetAllNoneDeleted()
                .Where(x => x.AcademicSessionId == req.AcademicSessionId &&
                            x.ClassId == req.ClassId &&
                            x.SubjectId == req.SubjectId &&
                            x.ExamComponentId == req.ExamComponentId)
                .ToListAsync(cancellationToken);

            foreach (var s in req.StudentMarks)
            {
                var existing = existingMarks.FirstOrDefault(x => x.StudentId == s.StudentId);
                var markVal = s.MarksObtained ?? 0;
                if (existing != null)
                {
                    existing.MarksObtained = markVal;
                    existing.IsAbsent = s.IsAbsent;
                    await _unitOfWork.StudentExamMarkRepository.UpdateAsync(existing);
                }
                else
                {
                    var newMark = new StudentExamMark
                    {
                        AcademicSessionId = req.AcademicSessionId,
                        ClassId = req.ClassId,
                        SectionId = req.SectionId,
                        StudentId = s.StudentId,
                        SubjectId = req.SubjectId,
                        ExamComponentId = req.ExamComponentId,
                        MarksObtained = markVal,
                        IsAbsent = s.IsAbsent,
                        IsActive = true
                    };
                    await _unitOfWork.StudentExamMarkRepository.AddAsync(newMark);
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(true, "Batch marks saved successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(SaveExamComponentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var comp = request.Component;
            if (comp.Id != Guid.Empty)
            {
                var existing = await _unitOfWork.ExamComponentRepository.GetSingleNoneDeletedAsync(x => x.Id == comp.Id);
                if (existing == null)
                    return Result.Fail(StatusCodes.Status404NotFound, "Exam component not found.");

                existing.TermName = comp.TermName;
                existing.ComponentName = comp.ComponentName;
                existing.MaxMarks = comp.MaxMarks;
                existing.OrderIndex = comp.OrderIndex;
                await _unitOfWork.ExamComponentRepository.UpdateAsync(existing);
            }
            else
            {
                var newEntity = new ExamComponent
                {
                    AcademicSessionId = comp.AcademicSessionId,
                    TermName = comp.TermName,
                    ComponentName = comp.ComponentName,
                    MaxMarks = comp.MaxMarks,
                    OrderIndex = comp.OrderIndex,
                    IsActive = true
                };
                await _unitOfWork.ExamComponentRepository.AddAsync(newEntity);
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(true, "Exam component saved successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(GetExamComponentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await EnsureExamComponentsSeededAsync(request.AcademicSessionId, cancellationToken);

            var query = _unitOfWork.ExamComponentRepository.GetAllNoneDeleted();
            if (request.AcademicSessionId.HasValue && request.AcademicSessionId.Value != Guid.Empty)
            {
                query = query.Where(x => x.AcademicSessionId == request.AcademicSessionId || x.AcademicSessionId == null);
            }

            var list = await query.OrderBy(x => x.OrderIndex)
                .Select(x => new ExamComponentDto
                {
                    Id = x.Id,
                    AcademicSessionId = x.AcademicSessionId,
                    TermName = x.TermName,
                    ComponentName = x.ComponentName,
                    MaxMarks = x.MaxMarks,
                    OrderIndex = x.OrderIndex
                })
                .ToListAsync(cancellationToken);

            return Result.Success(list);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(GetBatchMarksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all admitted students in this class and session
            var admissionQuery = _unitOfWork.AdmissionRepository.GetAllNoneDeleted(false, true)
                .Include(x => x.Student)
                .Where(x => x.AcademicSessionId == request.AcademicSessionId && x.ClassId == request.ClassId && !x.IsCancelled);

            if (request.SectionId.HasValue && request.SectionId.Value != Guid.Empty)
            {
                admissionQuery = admissionQuery.Where(x => x.SectionId == request.SectionId);
            }

            var admissions = await admissionQuery.OrderBy(x => x.RollNo).ToListAsync(cancellationToken);

            // Existing marks
            var existingMarks = await _unitOfWork.StudentExamMarkRepository.GetAllNoneDeleted()
                .Where(x => x.AcademicSessionId == request.AcademicSessionId &&
                            x.ClassId == request.ClassId &&
                            x.SubjectId == request.SubjectId &&
                            x.ExamComponentId == request.ExamComponentId)
                .ToListAsync(cancellationToken);

            var result = admissions.Select(a =>
            {
                var markRecord = existingMarks.FirstOrDefault(m => m.StudentId == a.StudentId);
                return new BatchStudentMarkItem
                {
                    StudentId = a.StudentId,
                    StudentName = a.Student?.FullName ?? "Unknown",
                    RollNo = a.RollNo,
                    MarksObtained = markRecord?.MarksObtained,
                    IsAbsent = markRecord?.IsAbsent ?? false
                };
            }).ToList();

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(GetStudentMarksheetQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await EnsureExamComponentsSeededAsync(request.AcademicSessionId, cancellationToken);

            // 1. Student & Admission details
            var admission = await _unitOfWork.AdmissionRepository.GetAllNoneDeleted(false, true)
                .Include(x => x.Student)
                .Include(x => x.Class)
                .Include(x => x.Section)
                .Include(x => x.AcademicSession)
                .FirstOrDefaultAsync(x => (request.AcademicSessionId == Guid.Empty || x.AcademicSessionId == request.AcademicSessionId) &&
                                          (x.StudentId == request.StudentId || x.Id == request.StudentId), cancellationToken)
                ?? await _unitOfWork.AdmissionRepository.GetAllNoneDeleted(false, true)
                .Include(x => x.Student)
                .Include(x => x.Class)
                .Include(x => x.Section)
                .Include(x => x.AcademicSession)
                .FirstOrDefaultAsync(x => x.StudentId == request.StudentId || x.Id == request.StudentId, cancellationToken);

            var studentName = admission?.Student?.FullName ?? "Student";
            var rollNo = admission?.RollNo ?? "";
            var className = admission?.Class?.ClassName ?? "Class";
            var sectionName = admission?.Section?.SectionName ?? "";
            var sessionName = admission?.AcademicSession?.SessionName ?? "Academic Session";
            var targetSessionId = request.AcademicSessionId != Guid.Empty ? request.AcademicSessionId : (admission != null ? admission.AcademicSessionId : Guid.Empty);
            var targetClassId = request.ClassId != Guid.Empty ? request.ClassId : (admission != null ? admission.ClassId : Guid.Empty);

            // 2. Exam Components
            var components = await _unitOfWork.ExamComponentRepository.GetAllNoneDeleted()
                .Where(x => x.AcademicSessionId == targetSessionId || x.AcademicSessionId == null)
                .OrderBy(x => x.OrderIndex)
                .Select(x => new ExamComponentDto
                {
                    Id = x.Id,
                    AcademicSessionId = x.AcademicSessionId,
                    TermName = x.TermName,
                    ComponentName = x.ComponentName,
                    MaxMarks = x.MaxMarks,
                    OrderIndex = x.OrderIndex
                })
                .ToListAsync(cancellationToken);

            if (components.Count == 0)
            {
                components = await _unitOfWork.ExamComponentRepository.GetAllNoneDeleted()
                    .OrderBy(x => x.OrderIndex)
                    .Select(x => new ExamComponentDto
                    {
                        Id = x.Id,
                        AcademicSessionId = x.AcademicSessionId,
                        TermName = x.TermName,
                        ComponentName = x.ComponentName,
                        MaxMarks = x.MaxMarks,
                        OrderIndex = x.OrderIndex
                    })
                    .ToListAsync(cancellationToken);
            }

            // 3. Subjects assigned to this class
            var classSubjectList = await _unitOfWork.ClassSubjectRepository.GetAllNoneDeleted()
                .Where(x => x.ClassId == targetClassId && x.IsActive)
                .Include(x => x.Subject)
                .OrderBy(x => x.OrderIndex)
                .Select(x => x.Subject)
                .Where(x => x != null && !x.IsDeleted)
                .ToListAsync(cancellationToken);

            List<Subject> classSubjects = classSubjectList.Where(x => x != null).Select(x => x!).ToList();

            // If class has no subjects assigned yet, fallback to all active subjects
            if (classSubjects.Count == 0)
            {
                classSubjects = await _unitOfWork.SubjectRepository.GetAllNoneDeleted()
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.SubjectName)
                    .ToListAsync(cancellationToken);
            }

            // 4. Grading Scale
            var gradingScale = await _unitOfWork.GradingScaleRepository.GetAllNoneDeleted()
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.IsDefault, cancellationToken)
                ?? await _unitOfWork.GradingScaleRepository.GetAllNoneDeleted()
                .Include(x => x.Details)
                .FirstOrDefaultAsync(cancellationToken);

            var gradingDetails = gradingScale?.Details.OrderByDescending(x => x.MinMark).ToList() ?? new List<GradingScaleDetail>();

            // 5. Existing marks scored by the student
            var targetStudentId = admission != null ? admission.StudentId : request.StudentId;
            var existingMarks = await _unitOfWork.StudentExamMarkRepository.GetAllNoneDeleted()
                .Where(x => (targetSessionId == Guid.Empty || x.AcademicSessionId == targetSessionId) &&
                            (x.StudentId == targetStudentId || (admission != null && x.StudentId == admission.Id)))
                .ToListAsync(cancellationToken);

            // Component matching IDs
            var ct1Id = components.FirstOrDefault(c => IsComponentMatch(c.ComponentName, "CT-1", "CT1", "CT 1"))?.Id;
            var ct2Id = components.FirstOrDefault(c => IsComponentMatch(c.ComponentName, "CT-2", "CT2", "CT 2"))?.Id;
            var hyId = components.FirstOrDefault(c => c.ComponentName.Contains("Half", StringComparison.OrdinalIgnoreCase))?.Id;
            var ct3Id = components.FirstOrDefault(c => IsComponentMatch(c.ComponentName, "CT-3", "CT3", "CT 3"))?.Id;
            var ct4Id = components.FirstOrDefault(c => IsComponentMatch(c.ComponentName, "CT-4", "CT4", "CT 4"))?.Id;
            var yearlyId = components.FirstOrDefault(c => (c.ComponentName.Contains("Yearly", StringComparison.OrdinalIgnoreCase) || c.ComponentName.Contains("Final", StringComparison.OrdinalIgnoreCase)) && !c.ComponentName.Contains("Half", StringComparison.OrdinalIgnoreCase))?.Id;

            var subjectRows = new List<MarksheetSubjectRowDto>();

            foreach (var sub in classSubjects)
            {
                var subMarks = existingMarks.Where(m => m.SubjectId == sub.Id).ToList();

                decimal? ct1 = subMarks.FirstOrDefault(m =>
                    (ct1Id.HasValue && m.ExamComponentId == ct1Id.Value) ||
                    IsComponentMatch(components.FirstOrDefault(c => c.Id == m.ExamComponentId)?.ComponentName, "CT-1", "CT1", "CT 1")
                )?.MarksObtained;

                decimal? ct2 = subMarks.FirstOrDefault(m =>
                    (ct2Id.HasValue && m.ExamComponentId == ct2Id.Value) ||
                    IsComponentMatch(components.FirstOrDefault(c => c.Id == m.ExamComponentId)?.ComponentName, "CT-2", "CT2", "CT 2")
                )?.MarksObtained;

                decimal? halfYearly = subMarks.FirstOrDefault(m =>
                    (hyId.HasValue && m.ExamComponentId == hyId.Value) ||
                    (components.FirstOrDefault(c => c.Id == m.ExamComponentId)?.ComponentName.Contains("Half", StringComparison.OrdinalIgnoreCase) == true)
                )?.MarksObtained;

                decimal term1Total = (ct1 ?? 0) + (ct2 ?? 0) + (halfYearly ?? 0);

                decimal? ct3 = subMarks.FirstOrDefault(m =>
                    (ct3Id.HasValue && m.ExamComponentId == ct3Id.Value) ||
                    IsComponentMatch(components.FirstOrDefault(c => c.Id == m.ExamComponentId)?.ComponentName, "CT-3", "CT3", "CT 3")
                )?.MarksObtained;

                decimal? ct4 = subMarks.FirstOrDefault(m =>
                    (ct4Id.HasValue && m.ExamComponentId == ct4Id.Value) ||
                    IsComponentMatch(components.FirstOrDefault(c => c.Id == m.ExamComponentId)?.ComponentName, "CT-4", "CT4", "CT 4")
                )?.MarksObtained;

                decimal? yearly = subMarks.FirstOrDefault(m =>
                    (yearlyId.HasValue && m.ExamComponentId == yearlyId.Value) ||
                    (components.FirstOrDefault(c => c.Id == m.ExamComponentId)?.ComponentName.Contains("Yearly", StringComparison.OrdinalIgnoreCase) == true)
                )?.MarksObtained;

                decimal term2Total = (ct3 ?? 0) + (ct4 ?? 0) + (yearly ?? 0);

                decimal overallAverage = Math.Round((term1Total + term2Total) / 2m, 2);

                var matchedGrade = gradingDetails.FirstOrDefault(g => overallAverage >= g.MinMark && overallAverage <= g.MaxMark);
                var gradeLetter = matchedGrade?.LetterGrade ?? (overallAverage >= 80 ? "A+" : overallAverage >= 70 ? "A" : overallAverage >= 60 ? "A-" : overallAverage >= 50 ? "B" : overallAverage >= 40 ? "C" : overallAverage >= 33 ? "D" : "F");
                var gradePoint = matchedGrade?.GradePoint ?? (overallAverage >= 80 ? 5.0m : overallAverage >= 70 ? 4.0m : overallAverage >= 60 ? 3.5m : overallAverage >= 50 ? 3.0m : overallAverage >= 40 ? 2.0m : overallAverage >= 33 ? 1.0m : 0.0m);

                subjectRows.Add(new MarksheetSubjectRowDto
                {
                    SubjectId = sub.Id,
                    SubjectName = sub.SubjectName,
                    CT1 = ct1,
                    CT2 = ct2,
                    HalfYearly = halfYearly,
                    Term1Total = term1Total,
                    CT3 = ct3,
                    CT4 = ct4,
                    YearlyExam = yearly,
                    Term2Total = term2Total,
                    OverallAverage = overallAverage,
                    Grade = gradeLetter,
                    GradePoint = gradePoint
                });
            }

            var summary = new MarksheetSummaryRowDto
            {
                TotalCT1 = subjectRows.Sum(x => x.CT1 ?? 0),
                TotalCT2 = subjectRows.Sum(x => x.CT2 ?? 0),
                TotalHalfYearly = subjectRows.Sum(x => x.HalfYearly ?? 0),
                TotalTerm1 = subjectRows.Sum(x => x.Term1Total),
                TotalCT3 = subjectRows.Sum(x => x.CT3 ?? 0),
                TotalCT4 = subjectRows.Sum(x => x.CT4 ?? 0),
                TotalYearlyExam = subjectRows.Sum(x => x.YearlyExam ?? 0),
                TotalTerm2 = subjectRows.Sum(x => x.Term2Total),
                OverallAverage = subjectRows.Count > 0 ? Math.Round(subjectRows.Average(x => x.OverallAverage), 2) : 0,
                FinalGPA = subjectRows.Count > 0 ? Math.Round(subjectRows.Average(x => x.GradePoint), 2) : 0
            };

            var finalMatch = gradingDetails.FirstOrDefault(g => summary.OverallAverage >= g.MinMark && summary.OverallAverage <= g.MaxMark);
            summary.FinalGrade = finalMatch?.LetterGrade ?? (summary.OverallAverage >= 80 ? "A+" : summary.OverallAverage >= 70 ? "A" : summary.OverallAverage >= 60 ? "A-" : summary.OverallAverage >= 50 ? "B" : summary.OverallAverage >= 40 ? "C" : summary.OverallAverage >= 33 ? "D" : "F");

            var scaleLegend = gradingDetails.Select(g => new GradingScaleLegendItem
            {
                MarksRange = $"{g.MinMark:0}–{g.MaxMark:0}",
                LetterGrade = g.LetterGrade,
                GPA = g.GradePoint
            }).ToList();

            var response = new StudentMarksheetResponse
            {
                AcademicSession = sessionName,
                AcademicSessionId = request.AcademicSessionId,
                StudentName = studentName,
                StudentRoll = rollNo,
                StudentId = targetStudentId,
                ClassName = className,
                ClassId = request.ClassId,
                SectionName = sectionName,
                SectionId = request.SectionId,
                Components = components,
                Subjects = subjectRows,
                Summary = summary,
                GradingScaleLegend = scaleLegend
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private async Task EnsureExamComponentsSeededAsync(Guid? sessionId, CancellationToken cancellationToken)
    {
        var anyComp = await _unitOfWork.ExamComponentRepository.GetAllNoneDeleted().AnyAsync(cancellationToken);
        if (!anyComp)
        {
            var defaults = new List<ExamComponent>
            {
                new() { TermName = "Term-1", ComponentName = "CT-1", MaxMarks = 15, OrderIndex = 1, IsActive = true, AcademicSessionId = sessionId },
                new() { TermName = "Term-1", ComponentName = "CT-2", MaxMarks = 15, OrderIndex = 2, IsActive = true, AcademicSessionId = sessionId },
                new() { TermName = "Term-1", ComponentName = "Half-Yearly", MaxMarks = 70, OrderIndex = 3, IsActive = true, AcademicSessionId = sessionId },
                new() { TermName = "Term-2", ComponentName = "CT-3", MaxMarks = 15, OrderIndex = 4, IsActive = true, AcademicSessionId = sessionId },
                new() { TermName = "Term-2", ComponentName = "CT-4", MaxMarks = 15, OrderIndex = 5, IsActive = true, AcademicSessionId = sessionId },
                new() { TermName = "Term-2", ComponentName = "Yearly Exam", MaxMarks = 70, OrderIndex = 6, IsActive = true, AcademicSessionId = sessionId },
            };

            foreach (var d in defaults)
            {
                await _unitOfWork.ExamComponentRepository.AddAsync(d);
            }
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }

    public async Task<IResult> Handle(GetStudentsByClassQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.AdmissionRepository.GetAllNoneDeleted(false, true)
                .Include(x => x.Student)
                .Where(x => x.AcademicSessionId == request.AcademicSessionId && x.ClassId == request.ClassId && !x.IsCancelled);

            if (request.SectionId.HasValue && request.SectionId.Value != Guid.Empty)
            {
                query = query.Where(x => x.SectionId == request.SectionId);
            }

            var list = await query.OrderBy(x => x.RollNo)
                .Select(x => new StudentDropdownItem
                {
                    StudentId = x.StudentId,
                    AdmissionId = x.Id,
                    StudentName = x.Student != null ? x.Student.FullName ?? "Student" : "Student",
                    RollNo = x.RollNo
                })
                .ToListAsync(cancellationToken);

            return Result.Success(list);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private static bool IsComponentMatch(string? componentName, params string[] targets)
    {
        if (string.IsNullOrWhiteSpace(componentName)) return false;
        var clean = componentName.Replace("-", "").Replace(" ", "").Trim().ToLowerInvariant();
        foreach (var t in targets)
        {
            var cleanT = t.Replace("-", "").Replace(" ", "").Trim().ToLowerInvariant();
            if (clean == cleanT || clean.Contains(cleanT)) return true;
        }
        return false;
    }
}
