using SchoolManagementSystem.Application.GS.Roles.Commands;

namespace SchoolManagementSystem.Application.GS.Roles.FluentValidations;
public class InsertRoleCommandValidator : AbstractValidator<InsertRoleCommand>
{
    public InsertRoleCommandValidator()
    {

        //RuleFor(x => x.Role.Name)
        //     .NotNull()
        //    .NotEmpty()
        //    .WithMessage("Role name is required.");
    }
}
