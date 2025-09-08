using GetzTest.Application.Models;

namespace GetzTest.Application.CQRS.Queries;

public interface IAccountQueries
{
    Task<AccountDto?> GetAccountAsync(Guid id);
    Task<AccountDto?> GetAccountByEmailAsync(string email);
    Task<AccountDto?> GetAccountByNameAsync(string name);
    Task<PagedResult<AccountDto>> GetAccountsAsync(int pageNumber, int pageSize);
}
