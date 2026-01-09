using SchoolManagementSystem.Application.GS.Users.Commands;

namespace SchoolManagementSystem.Application.GS.Users.FluentValidations;
public class InsertUserCommandValidator : AbstractValidator<InsertUserCommand>
{
    public InsertUserCommandValidator()
    {

        RuleFor(x => x.User.FirstName)
             .NotNull()
            .NotEmpty()
            .WithMessage("User FirstName is required.");
        RuleFor(x => x.User.LastName)
            .NotNull()
           .NotEmpty()
           .WithMessage("User LastName is required.");
        RuleFor(x => x.User.Email)
            .NotNull()
           .NotEmpty()
           .WithMessage("User Email is required.");
        RuleFor(x => x.User.PhoneNumber)
            .NotNull()
           .NotEmpty()
           .WithMessage("User PhoneNumber is required.");
        RuleFor(x => x.User.UserType)
            .NotNull()
           .NotEmpty()
           .WithMessage("User UserType is required.");
       
    }
}
