using GetzTest.Domain.Entities;

namespace GetzTest.Infrastructure.Repositories;

public interface IAccountRepository : IBaseRepository<Account>
{
    public bool VerifyPassword(Account entity, string hashedPassword, string verifiedPassword);
    public Account UpdatePassword(Account entity, string newPassword);
    public Task<Account?> FindByIdAsync(Guid id);
    public Task<Account?> FindByEmailAsync(string email);
    public Task<Account?> FindByNameAsync(string name);
    public void SoftDeleted(Account entity);

    public Task<IEnumerable<Account>?> GetAllAsync();
}
