using GetzTest.Application.CQRS.Queries;
using GetzTest.Application.Jwts;
using MediatR;

namespace GetzTest.Application.Apis.Account;

public class AccountsService
{
    public AccountsService(IMediator mediator, IJwtManager jwtManager, IAccountQueries accountQueries)
    {
        JwtManager = jwtManager;
        AccountQueries = accountQueries;
        Mediator = mediator;
    }

    public IMediator Mediator { get; }
    public IJwtManager JwtManager { get; }
    public IAccountQueries AccountQueries { get; }
}
