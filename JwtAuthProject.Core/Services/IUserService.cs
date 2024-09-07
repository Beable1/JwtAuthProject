using JwtAuthProject.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Core.Services
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);

        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);

        Task<Response<NoContentDto>> CreateUserRoles(string username);
        
        
    }
}
