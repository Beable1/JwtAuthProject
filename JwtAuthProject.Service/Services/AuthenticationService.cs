using JwtAuthProject.Core.Configuration;
using JwtAuthProject.Core.Dtos;
using JwtAuthProject.Core.Models;
using JwtAuthProject.Core.Repositories;
using JwtAuthProject.Core.Services;
using JwtAuthProject.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRTRepository;

        public AuthenticationService(IOptions<List<Client>> clients, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRTRepository)
        {
            _clients = clients.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRTRepository = userRTRepository;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if(loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            
            var user=await _userManager.FindByEmailAsync(loginDto.Email);

            
            if (user == null) return Response<TokenDto>.Fail("Email or password is wrong",400,true);

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                
                return Response<TokenDto>.Fail("Email or password is wrong", 400, true);
            }

            var token =_tokenService.CreateToken(user);
            var userRefreshToken= await _userRTRepository.Where(x=>x.UserId==user.Id).SingleOrDefaultAsync();    

            if (userRefreshToken == null)
            {
                await _userRTRepository.AddAsync(new UserRefreshToken { UserId = user.Id ,Code=token.RefreshToken,Expiration=token.RefreshTokenExpiration});
            }else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token, 200);



        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
            }

            var token=_tokenService.CreateTokenByClient(client);    
            return Response<ClientTokenDto>.Success(token, 200);

        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            
            var existRefreshToken=await _userRTRepository.Where(x=>x.Code == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null)
            {
                return Response<TokenDto>.Fail("Refresh token not found", 404,true);
            }

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);
            if (user == null)
            {
                return Response<TokenDto>.Fail("User Id not found", 404, true);
            }

            var tokenDto=_tokenService.CreateToken(user);

            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(tokenDto, 200);


        }

        public async Task<Response<NoContentDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken= await _userRTRepository.Where(x=>x.Code==refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null)
            {
                return Response<NoContentDto>.Fail("Refresh Token Not Found",404,true);
            }

            _userRTRepository.Remove(existRefreshToken);
            await _unitOfWork.CommitAsync();
            return Response<NoContentDto>.Success(200);
        }

        
    }
}
