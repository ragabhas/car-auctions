using System;
using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);

        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        var claims = await _userManager.GetClaimsAsync(user);

        var newClaims = new List<Claim>
        {
            new Claim("username", user.UserName),
        };

        context.IssuedClaims.AddRange(newClaims);
        context.IssuedClaims.Add(claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name));
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}
