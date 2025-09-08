using GetzTest.Application.Models;
using GetzTest.Infrastructure;
using GetzTest.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public async Task<PagedResult<AccountDto>> GetAccountsAsync(int pageNumber, int pageSize)
    {
        var query = _repository.GetAllAsync();
        int totalItems = await query.CountAsync();

        var accounts = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var accountsDto = new List<AccountDto>();

        foreach (var account in accounts)
        {
            accountsDto.Add(new AccountDto
            {
                Id = account.Id,
                UserName = account.UserName,
                Email = account.Email,
            });
        }

        return new PagedResult<AccountDto>
        {
            Items = accountsDto,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
