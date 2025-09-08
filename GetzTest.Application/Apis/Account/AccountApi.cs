using GetzTest.Application.CQRS.Commands;
using GetzTest.Application.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ValidationException = FluentValidation.ValidationException;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace GetzTest.Application.Apis.Account;

public static class AccountApiExtensions
{
    public static IEndpointRouteBuilder MapAccountApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("/api/identity/v1")
            .MapAccountApi()
            .RequireAuthorization()
            .WithTags("Account Api");

        return builder;
    }

    private static RouteGroupBuilder MapAccountApi(this RouteGroupBuilder group)
    {
        group.MapPost("auth/login", AccountApi.LoginAsync).AllowAnonymous();
        group.MapPost("auth/register", AccountApi.RegisterAsync).AllowAnonymous();

        group.MapGet("accounts", AccountApi.GetAccountsAsync);
        group.MapGet("accounts/{id:guid}", AccountApi.GetAccountAsync);
        group.MapGet("accounts/by-email/{email}", AccountApi.GetAccountByEmailAsync);
        group.MapGet("accounts/by-name/{name}", AccountApi.GetAccountByNameAsync);

        return group;
    }
}

public abstract class AccountApi
{
    public static async Task<Results<Ok<string>, BadRequest<LoginResult>>> LoginAsync(LoginCommand command,
        [AsParameters] AccountsService accountsService, HttpContext httpContext)
    {
        var result = await accountsService.Mediator.Send(command);

        if (result.IsValid)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };

            httpContext.Response.Cookies.Append("token", result.JwtModel?.RefreshToken ?? "", cookieOptions);

            return TypedResults.Ok(result.JwtModel?.AccessToken);
        }

        return TypedResults.BadRequest(result);
    }

    public static async Task<Results<Ok, BadRequest<IEnumerable<ValidationFailure>>, ProblemHttpResult>> RegisterAsync(
        RegisterCommand registerCommand,
        [AsParameters] AccountsService accountsService)
    {
        try
        {
            var result = await accountsService.Mediator.Send(registerCommand);

            if (result) return TypedResults.Ok();
        }
        catch (ApplicationException e)
        {
            if (e.InnerException is ValidationException validation)
            {
                return TypedResults.BadRequest(validation.Errors);
            }
        }

        return TypedResults.Problem("Registration failed", statusCode: 500);
    }

    public static async Task<Ok<PagedResult<AccountDto>>> GetAccountsAsync(
        [FromQuery] int pageNumber, [FromQuery] int pageSize,
        [AsParameters] AccountsService accountsService)
    {
        return TypedResults.Ok(await accountsService.AccountQueries.GetAccountsAsync(pageNumber, pageSize));
    }

    public static async Task<Ok<AccountDto>> GetAccountAsync(
        [AsParameters] AccountsService accountsService, [FromRoute] Guid id)
    {
        return TypedResults.Ok(await accountsService.AccountQueries.GetAccountAsync(id));
    }

    public static async Task<Ok<AccountDto>> GetAccountByEmailAsync(
        [AsParameters] AccountsService accountsService, [FromRoute] string email)
    {
        return TypedResults.Ok(await accountsService.AccountQueries.GetAccountByEmailAsync(email));
    }

    public static async Task<Ok<AccountDto>> GetAccountByNameAsync(
        [AsParameters] AccountsService accountsService, [FromRoute] string name)
    {
        return TypedResults.Ok(await accountsService.AccountQueries.GetAccountByNameAsync(name));
    }
}
