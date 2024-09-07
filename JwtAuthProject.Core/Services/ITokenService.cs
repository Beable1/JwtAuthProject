using JwtAuthProject.Core.Configuration;
using JwtAuthProject.Core.Dtos;
using JwtAuthProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserApp userApp);
        ClientTokenDto  CreateTokenByClient(Client cli);
    }
}
