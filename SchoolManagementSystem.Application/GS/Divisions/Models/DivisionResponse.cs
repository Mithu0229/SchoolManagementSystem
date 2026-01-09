namespace SchoolManagementSystem.Application.GS.Divisions.Models;

public class DivisionResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
