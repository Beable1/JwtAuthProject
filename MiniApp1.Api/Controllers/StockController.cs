using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp1.Api.Controllers
{
     [Authorize(Roles ="admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : Controller
    {
        [HttpGet]
        public IActionResult GetStock()
        {
            var username = HttpContext.User.Identity.Name;
            var userIdClaim=User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return Ok($" Stock işlemi > UserName:{username}-UserId{userIdClaim.Value}");
        }
    }
}
