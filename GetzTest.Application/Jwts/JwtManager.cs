using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using GetzTest.Domain.Entities;
using GetzTest.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace GetzTest.Application.Jwts;

public class JwtManager : IJwtManager
{
    private readonly JwtSettings _jwtSettings;
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly JwtKeyProvider _jwtKeyProvider;

    public JwtManager(IOptions<JwtSettings> jwtSettings, ApplicationDbContext applicationDbContext,
        JwtKeyProvider jwtKeyProvider)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        _jwtKeyProvider = jwtKeyProvider ?? throw new ArgumentNullException(nameof(jwtKeyProvider));
        _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
    }

    public async Task<string> GenerateRefreshToken(Guid accountId, Guid clientId, string? deviceInformation)
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var jwt = new Jwt(accountId, refreshToken, clientId, deviceInformation);

        await _applicationDbContext.Jwts.AddAsync(jwt);

        return refreshToken;
    }

    public async Task<bool> VerifyRefreshToken(string refreshToken)
    {
        // Need logic more
        return await _applicationDbContext.Jwts.AnyAsync(x => x.RefreshToken == refreshToken);
    }

    public IEnumerable<Claim> JwtClaims(Guid accountId, string username, string email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, accountId.ToString())
        };
        claims.AddRange(_jwtSettings.ValidAudiences.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)));

        return claims;
    }

    public Claim[] RegainClaims(string accessToken)
    {
        var accessTokenDecoded = DecodeAccessToken(accessToken);

        return accessTokenDecoded.Claims.ToArray();
    }

    private JwtSecurityToken DecodeAccessToken(string accessToken)
    {
        try
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException("Invalid token", ex);
        }
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        // Use Rsa algorithm instead of Hmac
        // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        // Need refactor
        // We must set private key only use in a class, not public get outside
        var key = _jwtKeyProvider.PrivateKey;
        key.KeyId = "1";

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresTime),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.RsaSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public List<Jwk> GenerateJwksToken()
    {
        // We need to set key id for the token
        var publicKey = _jwtKeyProvider.PublicKey;
        publicKey.KeyId = "1";

        var parameters = _jwtKeyProvider.PublicKey.Rsa.ExportParameters(false);
        var jwk = new List<Jwk>
        {
            new()
            {
                Kty = "RSA",
                Alg = "RS256",
                Kid = "1",
                Use = "sig",
                KeyOps = ["verify"],
                Modulus = Base64UrlEncoder.Encode(parameters.Modulus),
                PublicExponent = Base64UrlEncoder.Encode(parameters.Exponent)
            }
        };

        return jwk;
    }
}
