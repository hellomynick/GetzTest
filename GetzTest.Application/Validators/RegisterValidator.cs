using FluentValidation;
using GetzTest.Application.CQRS.Commands;
using GetzTest.Infrastructure.Repositories;

namespace GetzTest.Application.Validators;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    private readonly IAccountRepository _accountRepository;

    public RegisterValidator(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

        RuleFor(x => x.Email).Cascade(CascadeMode.Continue)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email is invalid")
            .MustAsync(async (email, _) =>
                await _accountRepository.FindByEmailAsync(email) == null)
            .WithMessage("Email already exists");

        RuleFor(x => x.UserName).Cascade(CascadeMode.Continue)
            .NotEmpty()
            .WithMessage("UserName is required");

        RuleFor(x => x.Password).Cascade(CascadeMode.Continue)
            .NotEmpty()
            .WithMessage("Password is required");

        RuleFor(x => x.ConfirmPassword).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ConfirmPassword is required");

        RuleFor(x => x.Password)
            .Equal(x => x.ConfirmPassword)
            .WithMessage("Passwords do not match");
    }
}
