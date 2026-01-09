using SchoolManagementSystem.Application.GS.Divisions.Commands;

namespace SchoolManagementSystem.Application.GS.Divisions.FluentValidations;
public class InsertDivisionCommandValidator : AbstractValidator<InsertDivisionCommand>
{
    public InsertDivisionCommandValidator()
    {

        RuleFor(x => x.Division.Name)
             .NotNull()
            .NotEmpty()
            .WithMessage("Division name is required.");
    }
}
