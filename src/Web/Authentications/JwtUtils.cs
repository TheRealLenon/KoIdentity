using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Tekoding.KoIdentity.Examples.API.Authentications;

public static class JwtUtils
{
    public static string GenerateJwtToken(Guid id)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = Encoding.ASCII.GetBytes("SECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRET");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", id.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public static Guid ValidateToken(string? token)
    {
        if (token == null)
        {
            return Guid.Empty;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = Encoding.ASCII.GetBytes("SECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRETSECRET");

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var secToken = (JwtSecurityToken)validatedToken;

            return Guid.Parse(secToken.Claims.First(x => x.Type == "id").Value);
        }
        catch (Exception e)
        {
            return Guid.Empty;
        }
    }
}