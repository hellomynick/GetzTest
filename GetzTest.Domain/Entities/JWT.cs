namespace GetzTest.Domain.Entities;

public class Jwt
{
    public Jwt()
    {
        Id = Guid.NewGuid();
    }

    public Jwt(Guid accountId, string refreshToken, Guid clientId, string? deviceInformation) : this()
    {
        AccountId = accountId;
        RefreshToken = refreshToken;
        ClientId = clientId;
        DeviceInformation = deviceInformation ?? "";
        Expires = DateTime.UtcNow.AddDays(14);
    }

    // Use ID here for future use save JWTs in a database (option of me)

    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid ClientId { get; set; }
    public string RefreshToken { get; set; }
    public string? DeviceInformation { get; set; }
    public DateTime Expires { get; private set; }
}
