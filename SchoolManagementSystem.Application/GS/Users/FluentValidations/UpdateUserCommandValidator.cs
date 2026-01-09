using SchoolManagementSystem.Application.GS.Users.Commands;

namespace SchoolManagementSystem.Application.GS.Users.FluentValidations;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.User.Id)
     .Cascade(CascadeMode.Stop)
     .NotNull()
     .NotEqual(Guid.Empty)
     .WithMessage("Id is required");

        RuleFor(x => x.User.FirstName)
             .NotNull()
            .NotEmpty()
            .WithMessage("User name is required.");
    }
}
