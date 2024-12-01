using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("auctionApp", "Auction App API"),
            new ApiScope("scope2"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman Client",
                AllowedScopes = {"openid", "profile", "auctionApp"},
                RedirectUris = {"https://www.getpostman.com/oauth2/callback"},
                ClientSecrets = {new Secret("NotASecret".Sha256())},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            },
            new Client
            {
                ClientId = "nextApp",
                ClientName = "nextjs app",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RequirePkce = false,
                AllowOfflineAccess = true,
                AllowedScopes = {"openid", "profile", "auctionApp"},
                RedirectUris = {"http://localhost:3000/api/auth/callback/id-server"},
                AccessTokenLifetime = 3600*24*30,
            }
        };
}
