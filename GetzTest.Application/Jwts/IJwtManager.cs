using System.Security.Claims;

namespace GetzTest.Application.Jwts;

public interface IJwtManager
{
    Task<string> GenerateRefreshToken(Guid accountId, Guid clientId, string? deviceInformation);
    Task<bool> VerifyRefreshToken(string refreshToken);
    IEnumerable<Claim> JwtClaims(Guid accountId, string username, string email);
    Claim[] RegainClaims(string accessToken);
    string GenerateAccessToken(IEnumerable<Claim> claims);
    List<Jwk> GenerateJwksToken();
}
