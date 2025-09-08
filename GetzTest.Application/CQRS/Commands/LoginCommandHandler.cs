using FluentValidation.Results;
using GetzTest.Application.Jwts;
using GetzTest.Application.Models;
using GetzTest.Infrastructure;
using GetzTest.Infrastructure.Repositories;
using MediatR;

namespace GetzTest.Application.CQRS.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtManager _jwtManager;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginCommandHandler(IAccountRepository accountRepository, IJwtManager jwtManager,
        ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = new LoginResult();

        var account = await _accountRepository.FindByEmailAsync(request.Email);
        if (account == null)
        {
            validationResult.Errors.Add(new ValidationFailure("Email", "Email is invalid"));

            return validationResult;
        }

        var isSamePassword = _accountRepository.VerifyPassword(account, account.Password, request.Password);
        if (!isSamePassword)
        {
            validationResult.Errors.Add(new ValidationFailure("Password", "Password is invalid"));

            return validationResult;
        }

        await validationResult.LoginValid(_jwtManager, _httpContextAccessor.HttpContext!, account);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return validationResult;
    }
}
