using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLayer.Core.DTOs.TokenDtos;
using NLayer.Core.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NLayer.Repository.Token
{
    public class TokenHandler : ITokenHandler
    {

        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Core.DTOs.TokenDtos.TokenDto CreateAccessToken(TokenInfo tokenInfo)
        {
            Core.DTOs.TokenDtos.TokenDto token = new Core.DTOs.TokenDtos.TokenDto();

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //Token geçerlilik süresi 60 dakika 
            token.Expiration = DateTime.UtcNow.AddHours(20);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: new[]
                {
                new Claim("Username", tokenInfo.Name),
                new Claim("Surname", tokenInfo.Surname),
                new Claim("UserId",Convert.ToString( tokenInfo.UserId)),
                //new Claim(ClaimTypes.Role, tokenInfo.Role)
                }
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            return token;
        }

    }
}
