using GetzTest.Application.Apis.Account;
using GetzTest.Application.Apis.Jwks;
using GetzTest.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.AddApplicationLayerServices();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapAccountApi();
app.MapApiJwksV1();

app.UseHttpsRedirection();
