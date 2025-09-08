using GetzTest.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GetzTest.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _identityDbContext;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AccountRepository(ApplicationDbContext dbContext, IPasswordHasher<Account> passwordHasher)
    {
        _identityDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task CreateAsync(Account entity)
    {
        _passwordHasher.HashPassword(entity, entity.Password);
        await _identityDbContext.Accounts.AddAsync(entity);
    }

    public Account Update(Account entity)
    {
        var entityUpdated = _identityDbContext.Accounts.Update(entity);

        return entityUpdated.Entity;
    }

    public bool Delete(Account entity)
    {
        _identityDbContext.Accounts.Remove(entity);
        return true;
    }

    public Account UpdatePassword(Account entity, string newPassword)
    {
        _passwordHasher.HashPassword(entity, newPassword);
        var entityUpdated = _identityDbContext.Accounts.Update(entity);

        return entityUpdated.Entity;
    }

    public bool VerifyPassword(Account entity, string hashedPassword, string verifyPassword)
    {
        var equalPassword = _passwordHasher.VerifyHashedPassword(entity, hashedPassword, verifyPassword);

        return equalPassword switch
        {
            PasswordVerificationResult.Success => true,
            PasswordVerificationResult.SuccessRehashNeeded => true,
            _ => false
        };
    }

    public async Task<Account?> FindByIdAsync(Guid id)
    {
        return await _identityDbContext.Accounts
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Account?> FindByEmailAsync(string email)
    {
        return await _identityDbContext.Accounts
            .SingleOrDefaultAsync(x => x.Email == email);
    }

    public async Task<Account?> FindByNameAsync(string name)
    {
        return await _identityDbContext.Accounts
            .SingleOrDefaultAsync(x => x.Email == name);
    }

    public void SoftDeleted(Account entity)
    {
        entity.IsDeleted = true;
        _identityDbContext.Accounts.Update(entity);
    }

    public async Task<IEnumerable<Account>?> GetAllAsync()
    {
        return await _identityDbContext.Accounts.ToListAsync();
    }


    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _identityDbContext.Accounts.AnyAsync(x => x.Id == id);
    }
}
