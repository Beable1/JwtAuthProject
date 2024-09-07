using AutoMapper;
using JwtAuthProject.Core.Dtos;
using JwtAuthProject.Core.Models;
using JwtAuthProject.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Service.Services
{
    public class UserService:IUserService
    {
        private readonly UserManager<UserApp> userManager;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(UserManager<UserApp> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user=new UserApp { Email= createUserDto.Email,UserName=createUserDto.UserName };
            var result= await userManager.CreateAsync(user,createUserDto.Password);    

            if(!result.Succeeded) {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }

            return Response<UserAppDto>.Success(mapper.Map<UserAppDto>(user), 200);

        }

        public async Task<Response<NoContentDto>> CreateUserRoles(string username)
        {
            if (!await roleManager.RoleExistsAsync("admin"))
            {

              await  roleManager.CreateAsync(new() { Name = "admin" });
              await  roleManager.CreateAsync(new() { Name = "moderator" });
            }
           

            var user= await userManager.FindByNameAsync(username);

            await userManager.AddToRolesAsync(user, new List<string> { "admin", "moderator" });

            return Response<NoContentDto>.Success(200);

        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var user=await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Response<UserAppDto>.Fail("UserName Not found",404,true);

            }

            return Response<UserAppDto>.Success(mapper.Map<UserAppDto>(user),200);   
        }
    }
}
