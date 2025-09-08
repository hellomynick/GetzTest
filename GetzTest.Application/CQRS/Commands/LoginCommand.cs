using GetzTest.Application.Models;
using MediatR;

namespace GetzTest.Application.CQRS.Commands;

public class LoginCommand : IRequest<LoginResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
