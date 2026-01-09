using SchoolManagementSystem.Application.GS.Tenants.Commands;

namespace SchoolManagementSystem.Application.GS.Tenants.FluentValidations;

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(x => x.Tenant!.Id)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEqual(Guid.Empty)
            .WithMessage("Tenant id is required");

        RuleFor(x => x.Tenant!.TenantName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Tenant name is required.")
            .Length(1, 400)
            .WithMessage("Tenant name length exceeded.");

        RuleFor(x => x.Tenant!.TenantEmail)
            .NotNull()
            .NotEmpty()
            .WithMessage("Tenant email is required.")
            .Length(1, 200)
            .WithMessage("Tenant email length exceeded.");
    }
}