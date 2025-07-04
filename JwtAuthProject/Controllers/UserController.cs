using JwtAuthProject.Core.Dtos;
using JwtAuthProject.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using System.Security.Claims;

namespace JwtAuthProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService userService;
        private readonly IChatService chatService;

        public UserController(IUserService userService, IChatService chatService)
        {
            this.userService = userService;
            this.chatService = chatService;
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


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return ActionResultInstance(await userService.GetUsersAsync(HttpContext.User.Identity.Name));
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetChatsByUserId(string userId)
        {
            var userName = HttpContext.User.Identity.Name;
            var userIdFromClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return ActionResultInstance(await chatService.GetChatsAsync(userIdFromClaims, userId));
        }

       

    }
}
