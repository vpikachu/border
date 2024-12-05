using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace border.api.Endpoints;

public static class SignEndpoint
{
  public static void MapSignEndpoints(this WebApplication app)
  {
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

    app.MapPost("/refresh",
          async ([FromBody] RefreshRequest refreshRequest, [FromServices] IServiceProvider sp, HttpContext http) =>
      {
        var timeProvider = sp.GetRequiredService<TimeProvider>();
        var bearerTokenOptions = sp.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
        var refreshTokenProtector = bearerTokenOptions.Get(BearerTokenDefaults.AuthenticationScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc
          || timeProvider.GetUtcNow() >= expiresUtc
        /*|| await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not BoardUser user*/)

        {
          await http.ChallengeAsync();
          return;
        }

        await http.SignInAsync(new ClaimsPrincipal(refreshTicket.Principal));
        return;
      }
    );
  }
}
