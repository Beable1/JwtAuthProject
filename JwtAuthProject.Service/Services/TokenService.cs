using JwtAuthProject.Core.Configuration;
using JwtAuthProject.Core.Dtos;
using JwtAuthProject.Core.Models;
using JwtAuthProject.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;
using SharedLibrary.Sevices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> userManager;
        private readonly CustomTokenOptions customTokenOptions;

        public TokenService(UserManager<UserApp> userManager,IOptions<CustomTokenOptions> options)
        {
            this.userManager = userManager;
            customTokenOptions = options.Value;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];

            using var rnd=RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }

        public TokenDto CreateToken(UserApp userApp)
        {
            var now = DateTime.Now;
            var accesTokenExpiration = now.AddMinutes(customTokenOptions.AccessTokenExpiration);
          
            var refreshTokenExpiration = now.AddMinutes(customTokenOptions.RefreshTokenExpiration);
 
            var securityKey=SignService.GetSymmetricSecurityKey(customTokenOptions.SecurityKey);


            SigningCredentials signingCredentials= new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);
            

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: customTokenOptions.Issuer,
                expires: accesTokenExpiration,
                notBefore: now,
                claims: GetClaim(userApp, customTokenOptions.Audience).Result,
                signingCredentials: signingCredentials
                );


            var handler = new JwtSecurityTokenHandler();
       
            var token=handler.WriteToken(jwtSecurityToken);
          
            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accesTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };
            return tokenDto;
        }

        private async Task<IEnumerable<Claim>> GetClaim(UserApp userApp,List<string> audiences)
        {

            var userRoles = await userManager.GetRolesAsync(userApp);
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
                new Claim(ClaimTypes.Name,userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

           


            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            userList.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));
            return userList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            new Claim(JwtRegisteredClaimNames.Sub,client.Id.ToString());
            return claims; 
        }


        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accesTokenExpiration = DateTime.Now.AddMinutes(customTokenOptions.AccessTokenExpiration);

            var securityKey = SignService.GetSymmetricSecurityKey(customTokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: customTokenOptions.Issuer,
                expires: accesTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials
                );

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,

                AccessTokenExpiration = accesTokenExpiration
            };
            return tokenDto;
        }
    }
}
