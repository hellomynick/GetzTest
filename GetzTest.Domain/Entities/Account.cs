namespace GetzTest.Domain.Entities;

public class Account
{
    public Account()
    {
        Id = Guid.NewGuid();
    }

    public Account(string email, string userName, string password) : this()
    {
        Email = email;
        UserName = userName;
        Password = password;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string UserName { get; private set; }
    public string Password { get; private set; }
    public bool IsDeleted { get; set; }

    public void AccountDelete()
    {
        IsDeleted = true;
    }
}
