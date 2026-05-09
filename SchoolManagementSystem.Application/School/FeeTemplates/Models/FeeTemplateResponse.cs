namespace SchoolManagementSystem.Application.School.FeeTemplates.Models;

public class FeeTemplateResponse
{
    public Guid Id { get; set; }
    public string TemplateName { get; set; }
    public Guid ClassId { get; set; }
    public string? ClassName { get; set; }
    public Guid? GroupId { get; set; }
    public string? GroupName { get; set; }
    public Guid? ShiftId { get; set; }
    public string? ShiftName { get; set; }
    public bool IsActive { get; set; }
    public IList<FeeTemplateDetailResponse> Details { get; set; } = new List<FeeTemplateDetailResponse>();
}

public class FeeTemplateDetailResponse
{
    public Guid Id { get; set; }
    public Guid FeeTemplateId { get; set; }
    public Guid FeeHeadId { get; set; }
    public string? FeeHeadName { get; set; }
    public decimal Amount { get; set; }
}
