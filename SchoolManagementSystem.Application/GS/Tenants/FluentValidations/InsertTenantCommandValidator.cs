using SchoolManagementSystem.Application.GS.Tenants.Commands;

namespace SchoolManagementSystem.Application.GS.Tenants.FluentValidations;

public class InsertTenantCommandValidator : AbstractValidator<InsertTenantCommand>
{
    public InsertTenantCommandValidator()
    {
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