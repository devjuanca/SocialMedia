using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Application.Entities;
using SocialMedia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Application.Services
{
    public interface ITokenGenerator
    {
        Task<DataToken> GenerateToken(SMUser user, string secretKey, string issuer, string audience);
    }


    public class TokenGenerator : ITokenGenerator
    {
       
        readonly UserManager<SocialMedia.Domain.Entities.SMUser> _userManager;
        public TokenGenerator(UserManager<SocialMedia.Domain.Entities.SMUser> userManager)
        {
            
            _userManager = userManager;
        }

        public async Task<DataToken> GenerateToken(SocialMedia.Domain.Entities.SMUser user, string secretKey, string issuer, string audience)
        {

            DataToken result = new DataToken {Id = user.Id, Roles=new List<string>()};
            //header
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims (Caracteristicas del usuario)

            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims_list = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var item in roles)
            {
                claims_list.Add(new Claim(ClaimTypes.Role, item));
                result.Roles.Add(item);
            }

            //Payload
            var payload = new JwtPayload
                (
                issuer,
                audience, claims_list,
                DateTime.UtcNow, //Desde cuando se puede usar
                DateTime.UtcNow.AddMinutes(60) //Cuando expira.
                );

            var token = new JwtSecurityToken(header, payload);
            result.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return result;


        }
    }
}
