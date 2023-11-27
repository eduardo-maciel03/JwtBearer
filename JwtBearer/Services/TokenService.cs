using JwtBearer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtBearer.Services;

public class TokenService
{
    public string Generate(User user)
    {
        // criando instância do JwtSecurityTokenHandler
        var handler = new JwtSecurityTokenHandler();

        // convertendo nossa chave para um array de bytes
        var key = Encoding.UTF8.GetBytes(Configuration.PrivateKey);

        // gerando credenciais, chave e tipo da assinatura
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key), 
            SecurityAlgorithms.HmacSha256Signature);

        // acrescentando claims, credenciais e tempo de expiração ao token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GeneretaClaims(user),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(2)
        };

        // gerando token
        var token = handler.CreateToken(tokenDescriptor);

        // gera uma string do token
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GeneretaClaims(User user)
    {
        var ci = new ClaimsIdentity();

        // gerando claims (tipo e valor)
        ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));

        foreach (var role in user.Roles)
            ci.AddClaim(new Claim(ClaimTypes.Role, role));

        return ci;
    }

}
