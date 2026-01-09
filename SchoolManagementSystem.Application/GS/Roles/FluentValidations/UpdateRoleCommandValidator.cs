using SchoolManagementSystem.Application.GS.Roles.Commands;

namespace SchoolManagementSystem.Application.GS.Roles.FluentValidations;
public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
     //   RuleFor(x => x.Role.Id)
     //.Cascade(CascadeMode.Stop)
     //.NotNull()
     //.NotEqual(Guid.Empty)
     //.WithMessage("Id is required");

     //   RuleFor(x => x.Role.Name)
     //        .NotNull()
     //       .NotEmpty()
     //       .WithMessage("Role name is required.");
    }
}
