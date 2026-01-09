using SchoolManagementSystem.Application.GS.Divisions.Commands;

namespace SchoolManagementSystem.Application.GS.Divisions.FluentValidations;
public class UpdateDivisionCommandValidator : AbstractValidator<UpdateDivisionCommand>
{
    public UpdateDivisionCommandValidator()
    {
        RuleFor(x => x.Division.Id)
     .Cascade(CascadeMode.Stop)
     .NotNull()
     .NotEqual(Guid.Empty)
     .WithMessage("Id is required");

        RuleFor(x => x.Division.Name)
             .NotNull()
            .NotEmpty()
            .WithMessage("Division name is required.");
    }
}
