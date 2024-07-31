using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using CoinFlo.BLL.Models.Auth;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CoinFlo.API.Helpers
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetJwtToken(LoginResponse loginResponse)
        {
            var cliams = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", loginResponse.Id.ToString()),
                new Claim("UserSecretKey", loginResponse.UserSecretKey.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    cliams,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: signIn
                );
            string jwtTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtTokenValue;
        }
    }
}
