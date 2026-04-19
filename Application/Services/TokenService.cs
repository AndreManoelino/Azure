using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CorporateIdentityManager.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CorporateIdentityManager.Application.Services
{
    public class TokenService (IConfiguration configuration)

    {
        private readonly IConfiguration _configuration = configuration;


        public string GerarToken(Usuario usuario)
        {
            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new Exception("JWT Key não configurada no appsettings");

            var key = Encoding.ASCII.GetBytes(jwtKey);

            List<Claim> claims =
            [
                new(ClaimTypes.Name, usuario.Nome),
                new(ClaimTypes.Email, usuario.Email),
                new("UPN", usuario.UPN),
                new("UserId", usuario.Id.ToString())
            ];

            var grupos = usuario.UsuarioGrupos?
                .Where(ug => ug.Grupo != null)
                .Select(ug => ug.Grupo!.Nome)
                .ToList() ?? [];

            foreach (var nomeGrupo in grupos)
            {
                claims.Add(new Claim("Grupo", nomeGrupo));
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}