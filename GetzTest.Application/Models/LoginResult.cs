using FluentValidation.Results;
using GetzTest.Application.Jwts;
using GetzTest.Domain.Entities;

namespace GetzTest.Application.Models;

public class LoginResult : ValidationResult
{
    public JwtDto? JwtModel { get; set; }

    public async Task LoginValid(IJwtManager jwtManager, HttpContext httpContext,
        Account account)
    {
        if (IsValid)
        {
            if (jwtManager == null) throw new ArgumentNullException(nameof(jwtManager));

            var claims = jwtManager.JwtClaims(account.Id, account.UserName, account.Email);
            var deviceInfo = httpContext.Request.Headers.UserAgent;

            JwtModel = new JwtDto
            {
                AccessToken = jwtManager.GenerateAccessToken(claims),
                RefreshToken = await jwtManager.GenerateRefreshToken(account.Id, Guid.NewGuid(), deviceInfo)
            };
        }
    }
}
