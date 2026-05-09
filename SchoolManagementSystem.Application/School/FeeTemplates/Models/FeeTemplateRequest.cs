namespace SchoolManagementSystem.Application.School.FeeTemplates.Models;

public class FeeTemplateRequest
{
    public Guid Id { get; set; }
    public string TemplateName { get; set; }
    public Guid ClassId { get; set; }
    public Guid? GroupId { get; set; }
    public Guid? ShiftId { get; set; }
    public bool IsActive { get; set; }
    public IList<FeeTemplateDetailRequest> Details { get; set; } = new List<FeeTemplateDetailRequest>();
}

public class FeeTemplateDetailRequest
{
    public Guid Id { get; set; }
    public Guid FeeHeadId { get; set; }
    public decimal Amount { get; set; }
}
