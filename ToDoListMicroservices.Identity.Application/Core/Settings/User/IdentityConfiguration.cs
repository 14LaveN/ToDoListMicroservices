using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace ToDoListMicroservices.Identity.Application.Core.Settings.User;

/// <summary>
/// Represents the identity configuration class.
/// </summary>
public static class IdentityConfiguration
{
    /// <summary>
    /// The api scopes list.
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("identityApi", "ToDoListMicroservices.Identity.Api")
        };

    /// <summary>
    /// The identity resources list.
    /// </summary>
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    /// <summary>
    /// The api resources list.
    /// </summary>
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("identityApi", "ToDoListMicroservices.Identity.Api")
        };

    /// <summary>
    /// The clients list.
    /// </summary>
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new()
            {
                ClientId = "to-do-list_microservices_identity",
                ClientName = "ToDoListMicroservices.Identity.Api",
                AllowedGrantTypes = GrantTypes.Hybrid,
                RequireClientSecret = false,
                RequirePkce = false,
                RedirectUris =
                {
                    "http://localhost:44460/signin-oidc"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:44460"
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:44460/signout-callback-oidc"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "identityApi"
                },
                AllowAccessTokensViaBrowser = true
            },
            new()
            {
                //TODO Change the uri.
                ClientId = "reactClient",
                ClientName = "React SPA Client",
                ClientUri = "http://localhost:4200",
                RequireConsent = false,
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                AllowAccessTokensViaBrowser = true,
                RedirectUris =
                {
                    "http://localhost:4200",
                    "http://localhost:4200/auth-callback",
                    "http://localhost:4200/silent-renew.html"
                },
                PostLogoutRedirectUris = new List<string>() { "http://localhost:4200" },
                AllowedCorsOrigins = { "http://localhost:4200" },
                AllowedScopes = new List<string>()
                { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile, 
                    "identityApi"
                }
            }
        };
}