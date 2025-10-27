using GoallyticsApp.Application.Dtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Persistance.Tools
{
    public static class JwtTokenGenerator
    {
        public static TokenResponseDto GenerateToken(CheckUserResponseDto dto)
        {
            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(dto.Role))
                claims.Add(new Claim(ClaimTypes.Role, dto.Role));
            claims.Add(new Claim(ClaimTypes.NameIdentifier,dto.Id.ToString()));
            if (!string.IsNullOrEmpty(dto.UserName))
                claims.Add(new Claim("Username",dto.UserName));

            SigningCredentials signingCredentials = new(
                key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.IssuerSigningKey)),
                algorithm: SecurityAlgorithms.HmacSha256);

            var expireDate = DateTime.UtcNow.AddMinutes(JwtTokenDefaults.ExpireMinutes);

            JwtSecurityToken jwtSecurityToken = new(
                issuer: JwtTokenDefaults.ValidIssuer,
                audience: JwtTokenDefaults.ValidAudience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireDate,
                signingCredentials: signingCredentials);
            JwtSecurityTokenHandler tokenHandler = new();
            //tokenHandler.WriteToken();
            return new TokenResponseDto(tokenHandler.WriteToken(jwtSecurityToken), expireDate);
        }

    }
}
