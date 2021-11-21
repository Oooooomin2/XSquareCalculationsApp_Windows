using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSquareCalculationsApi.Configs;

namespace XSquareCalculationsApi.Models
{
    public interface IResolveJwtAuthenticate
    {
        JwtResponse CreateJwtResponse(int userId, string userName);
    }

    public class ResolveJwtAuthenticate : IResolveJwtAuthenticate
    {
        private readonly ISystemDate _systemDate;

        public ResolveJwtAuthenticate(ISystemDate systemDate)
        {
            _systemDate = systemDate;
        }

        public JwtResponse CreateJwtResponse(int userId, string userName)
        {
            var key = Encoding.UTF8.GetBytes(JwtSettingsConfig.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(JwtRegisteredClaimNames.GivenName, userName) }),
                Audience = JwtSettingsConfig.SiteUrl,
                Issuer = JwtSettingsConfig.SiteUrl,
                SigningCredentials = new SigningCredentials
                (
                    key: new SymmetricSecurityKey(key),
                    algorithm: SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenText = tokenHandler.WriteToken(token);
            var now = _systemDate.GetSystemDate();
            return new JwtResponse
            {
                UserId = userId,
                IdToken = tokenText,
                ExpiredDateTime = now.AddDays(JwtSettingsConfig.ExpiredDay),
                CreatedTime = now
            };
        }
    }
}