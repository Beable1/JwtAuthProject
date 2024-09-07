using JwtAuthProject.Core.Dtos;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);

        Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);

        Task<Response<NoContentDto>> RevokeRefreshToken(string refreshToken);

        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}
