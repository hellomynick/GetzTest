using Microsoft.AspNetCore.Http.HttpResults;

namespace GetzTest.Application.Apis.Jwks;

public static class ApiJwks
{
    public static RouteGroupBuilder MapApiJwksV1(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/.well-known");

        api.MapGet("jwks.json", GetJwks).AllowAnonymous();
        api.MapGet("openid-configuration", OpenIdConfiguration).AllowAnonymous();

        return api;
    }

    private static async Task<Results<Ok<object>, BadRequest>> OpenIdConfiguration()
    {
        var baseUrl = "http://localhost:5036";
        var discovery = new
        {
            issuer = baseUrl,
            jwks_uri = $"{baseUrl}/.well-known/jwks.json",
            token_endpoint = $"{baseUrl}/api/identity/v1/auth/login",
            response_types_supported = new[] { "token" },
            subject_types_supported = new[] { "public" },
            id_token_signing_alg_values_supported = new[] { "RS256" }
        };

        return TypedResults.Ok((object)await Task.FromResult(discovery));
    }

    private static async Task<Results<Ok<object>, BadRequest>> GetJwks([AsParameters] JwksService service)
    {
        var jwks = await Task.FromResult(service.JwtManager.GenerateJwksToken());
        object response = new { keys = jwks };

        return TypedResults.Ok(response);
    }
}
