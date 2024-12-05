using System.Security.Claims;
using api.Services;
using border.api.Endpoints;
using border.api.Models;
using border.api.Repositories;
using border.api.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);
string? connectionStr = builder.Configuration.GetConnectionString("Main") ?? throw new Exception("Connection string is not provided");
builder.Services.AddSingleton<IDatabaseHelper>(new SqliteDatabaseHelper(connectionStr));
builder.Services.AddTransient<BoardsRepository>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = BearerTokenDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddBearerToken().AddGoogle(googleOptions =>
    {
        var authSettings = new AuthSettings();
        builder.Configuration.GetSection("Authentication").Bind(authSettings);
        if (string.IsNullOrEmpty(authSettings.Google.ClientId)
            || string.IsNullOrEmpty(authSettings.Google.ClientSecret)) throw new Exception("Google auth settings are not configured");
        googleOptions.ClientId = authSettings.Google.ClientId;
        googleOptions.ClientSecret = authSettings.Google.ClientSecret;
        googleOptions.SignInScheme = BearerTokenDefaults.AuthenticationScheme;
        googleOptions.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect(context.RedirectUri + "&prompt=consent");
            return Task.CompletedTask;
        };
    }
);
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.DoMigrate();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", (ClaimsPrincipal user) =>
{

    if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
    {
        return Results.Ok($"border REST API. Your account is {user.Identity.Name}");
    }
    else
        return Results.Ok("border REST API.");
});
app.MapBoardsEndpoints();
app.MapSignEndpoints();
app.MapOpenApi();

app.Run();
