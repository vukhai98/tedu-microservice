using Contracts.Identityserver;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations;
using Shared.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class TokenService : ITokenService
    {
        private readonly JwtSetting _jwtSetting;

        public TokenService(JwtSetting jwtSetting)
        {
            _jwtSetting = jwtSetting;
        }
        public TokenRespone GetToken(TokenRequest request)
        {
            var token = GenerateJwt();
            var result = new TokenRespone(token);

            return result;
        }

        private string GenerateJwt()
        {
            var result = GenerateEncryptedToken(GetSigningCredentials());
            return result;
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials)
        {
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSetting.Key);

            var result = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);

            return result;
        }
    }

}
