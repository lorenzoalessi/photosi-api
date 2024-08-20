using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using PhotosiApi.Dto.User;

namespace PhotosiApi.Utility;

public static class JwtUtils
{
    public static string GenerateJwtToken(UserDto userDto)
    {
        // Credentials
        var credentials = GetCredentials();

        // Claims
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };

        // Generazione token
        var token = new JwtSecurityToken(
            issuer: "photosi",
            audience: userDto.Username,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static JwtSecurityToken DecodeToken(string token) => new JwtSecurityTokenHandler().ReadJwtToken(token);
    
    public static bool IsValidToken(JwtSecurityToken token, UserDto userDto)
    {
        // Issuer
        if (token.Issuer != "photosi")
            return false;
        
        // Audience
        var audiences = token.Audiences.ToArray();
        if (audiences.Length != 1 || audiences.FirstOrDefault() != userDto.Username)
            return false;

        // Claims
        var claims = token.Claims.ToArray();
        var checkClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier && x.Value == userDto.Id.ToString());
        if (checkClaim == null)
            return false;
        
        // Expiration
        if (token.ValidTo < DateTime.Now)
            return false;

        // TODO: Perchè mi arriva nullo?
        // SigningCredentials
        // var checkCredentials = GetCredentials();
        // if (token.SigningCredentials != checkCredentials)
        //     return false;

        return true;
    }

    private static SigningCredentials GetCredentials()
    {
        // Crypto la chiave (generata casualmente) e ricavo le credentials per firmare il token
        // (chiave inline ma forse più corretto settarla in un setting nel file di properties)
        var securityKey = new SymmetricSecurityKey("zdf+1A34S1B+CJVfUIEeQ5cLkgZXFiJPDxpSPv47J2A="u8.ToArray());
        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }
}