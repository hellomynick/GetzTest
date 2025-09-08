namespace GetzTest.Application.Jwts;

public class JwtSettings
{
    public string ValidIssuer { get; set; } = null!;
    public string[] ValidAudiences { get; set; } = null!;
    public int ExpiresTime { get; set; }
    public string SecretKey { get; set; } = null!;
}
