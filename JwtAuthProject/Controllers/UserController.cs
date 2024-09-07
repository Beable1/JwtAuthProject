using JwtAuthProject.Core.Dtos;
using JwtAuthProject.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace JwtAuthProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            return ActionResultInstance(await userService.CreateUserAsync(createUserDto));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return ActionResultInstance(await userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }


        [HttpPost("CreateUseRoles/{username}")]
        public async Task<IActionResult> CreateUserRoles(string username)
        {
            
            return ActionResultInstance(await userService.CreateUserRoles(username));

        }

    }
}
