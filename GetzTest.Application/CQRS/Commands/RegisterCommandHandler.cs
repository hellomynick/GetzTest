using GetzTest.Domain.Entities;
using GetzTest.Infrastructure;
using GetzTest.Infrastructure.Repositories;
using MediatR;

namespace GetzTest.Application.CQRS.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ApplicationDbContext _dbContext;

    public RegisterCommandHandler(IAccountRepository accountRepository, ApplicationDbContext dbContext)
    {
        _accountRepository = accountRepository;
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var account = new Account(request.Email, request.UserName, request.Password);
        await _accountRepository.CreateAsync(account);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
