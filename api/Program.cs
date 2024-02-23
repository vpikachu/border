using System.Security.Claims;
using api.Services;
using border.api.Endpoints;
using border.api.Models;
using border.api.Repositories;
using border.api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);
string? connectionStr = builder.Configuration.GetConnectionString("Main") ?? throw new Exception("Connection string is not provided");
builder.Services.AddSingleton<IDatabaseHelper>(new SqliteDatabaseHelper(connectionStr));
builder.Services.AddTransient<BoardsRepository>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(googleOptions =>
    {
        var authSettings = new AuthSettings();
        builder.Configuration.GetSection("Authentication").Bind(authSettings);
        if (string.IsNullOrEmpty(authSettings.Google.ClientId)
            || string.IsNullOrEmpty(authSettings.Google.ClientSecret)) throw new Exception("Google auth settings are not configured");
        googleOptions.ClientId = authSettings.Google.ClientId;
        googleOptions.ClientSecret = authSettings.Google.ClientSecret;
        googleOptions.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect(context.RedirectUri + "&prompt=consent");
            return Task.CompletedTask;
        };
    }
);
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.DoMigrate();
app.MapBoardsEndpoints();

app.MapGet("/", (ClaimsPrincipal user) =>
{

    if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
    {
        return Results.Ok($"border REST API. Your account is {user.Identity.Name}");
    }
    else
        return Results.Ok("border REST API.");
});

app.MapGet("/signin", async signinApp =>
{
    await signinApp.ChallengeAsync(new AuthenticationProperties() { RedirectUri = "/" });
    return;
});

app.MapGet("/signout", async signinApp =>
{
    await signinApp.SignOutAsync(new AuthenticationProperties() { RedirectUri = "/" });
    return;
});

app.Run();
