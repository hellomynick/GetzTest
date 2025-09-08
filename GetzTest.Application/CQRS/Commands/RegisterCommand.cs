using MediatR;

namespace GetzTest.Application.CQRS.Commands;

public class RegisterCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
