using GetzTest.Application.Models;
using GetzTest.Infrastructure;
using GetzTest.Infrastructure.Repositories;

namespace GetzTest.Application.CQRS.Queries;

public class AccountQueries : IAccountQueries
{
    private readonly IAccountRepository _repository;

    public AccountQueries(IAccountRepository repository)
    {
        _repository = repository;
    }


    public async Task<AccountDto?> GetAccountAsync(Guid id)
    {
        var account = await _repository.FindByIdAsync(id);

        if (account == null) return null;

        return new AccountDto
        {
            UserName = account.UserName,
            Email = account.Email,
        };
    }

    public async Task<AccountDto?> GetAccountByEmailAsync(string email)
    {
        var account = await _repository.FindByEmailAsync(email);

        if (account == null) return null;

        return new AccountDto
        {
            UserName = account.UserName,
            Email = account.Email,
        };
    }

    public async Task<AccountDto?> GetAccountByNameAsync(string name)
    {
        var account = await _repository.FindByNameAsync(name);

        if (account == null) return null;

        return new AccountDto
        {
            UserName = account.UserName,
            Email = account.Email,
        };
    }

    public async Task<IEnumerable<AccountDto>?> GetAccountsAsync()
    {
        var accounts = await _repository.GetAllAsync();

        if (accounts == null) return null;

        var accountsDto = new List<AccountDto>();

        foreach (var account in accounts)
        {
            accountsDto.Add(new AccountDto
            {
                UserName = account.UserName,
                Email = account.Email,
            });
        }

        return accountsDto;
    }
}
