using FluentValidation;
using GetzTest.Application.Behaviors;
using GetzTest.Application.CQRS.Commands;
using GetzTest.Application.Jwts;
using GetzTest.Application.Validators;
using GetzTest.Domain.Entities;
using GetzTest.Infrastructure;
using GetzTest.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GetzTest.Application.Extensions;

public static class RegisterExtensions
{
    public static IHostApplicationBuilder AddApplicationLayerServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });

        // Add authentication and config
        builder.Services.AddScoped<IJwtManager, JwtManager>();
        builder.Services.AddSingleton(new JwtKeyProvider("Keys/private.pem", "Keys/public.pem"));
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            var validIssuer = jwtSettingsSection.GetRequireValue("ValidIssuer");
            var audiences = jwtSettingsSection.GetSection("ValidAudiences").Get<List<string>>() ??
                            throw new InvalidOperationException("Value not found");

            options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
            options.Authority = "http://localhost:5036";

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidIssuer = "http://localhost:5036",
                ValidAudiences = audiences,
                IssuerSigningKey = KeyExtension.ToRsaSecurityKey("Keys/public.pem", ImportType.Path),
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero,
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("Auth Failed: " + context.Exception);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated successfully!");
                    return Task.CompletedTask;
                }
            };
        });

        // FluentValidation
        builder.Services.AddScoped<IValidator<LoginCommand>, LoginValidator>();
        builder.Services.AddScoped<IValidator<RegisterCommand>, RegisterValidator>();

        // Infrastructure layer
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("AccountDb"));

        return builder;
    }
}
