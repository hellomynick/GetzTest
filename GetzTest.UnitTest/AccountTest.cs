using GetzTest.Application.CQRS.Commands;
using GetzTest.Domain.Entities;
using GetzTest.Infrastructure;
using GetzTest.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GetzTest.UnitTest;

public class AccountTest
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IAccountRepository> _repository;

    public AccountTest()
    {
        _repository = new Mock<IAccountRepository>();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("UsersDbTest")
            .Options;

        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CreateUser_ShouldAddUser()
    {
        var account = new Account("test@email.com", "test", "password");

        _repository.Setup(r => r.CreateAsync(It.IsAny<Account>()));

        await _repository.Object.CreateAsync(account);
        await _context.SaveChangesAsync();

        var result = await _context.Accounts.FirstOrDefaultAsync();
        
        Assert.NotNull(result);
        Assert.Equal("test", result.UserName);
    }

    [Fact]
    public async Task GetUser_ShouldReturnUser()
    {
        var account = new Account("test@email.com", "test", "password");
        await _repository.Object.CreateAsync(account);
        await _context.SaveChangesAsync();

        var result = await _context.Accounts.FindAsync(account.Id);

        Assert.NotNull(result);
        Assert.Equal("test", account.UserName);
    }
}
