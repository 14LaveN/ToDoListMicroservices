using System.IdentityModel.Tokens.Jwt;

namespace ToDoListMicroservices.Application.Core.Helpers.JWT;

/// <summary>
/// Represents the Get claim by jwt token class.
/// </summary>
public static class GetClaimByJwtToken
{
    /// <summary>
    /// Get name by JWT.
    /// </summary>
    /// <param name="token">The JWT.</param>
    /// <returns>Return the name from token.</returns>
    public static string GetNameByToken(string? token)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken tokenInfo = handler.ReadJwtToken(token);
        
        var claimsPrincipal = tokenInfo.Claims;
        
        var name = claimsPrincipal.FirstOrDefault(x=> x.Type == "name")?.Value;
        return name!;
    }
    
    /// <summary>
    /// Get identifier by JWT.
    /// </summary>
    /// <param name="token">The JWT.</param>
    /// <returns>Return the identifier from token.</returns>
    public static string GetIdByToken(string? token)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken tokenInfo = handler.ReadJwtToken(token);
        
        var claimsPrincipal = tokenInfo.Claims;
        
        var name = claimsPrincipal.FirstOrDefault(x=> x.Type == "nameid")?.Value;
        return name!;
    }
}