using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp2.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        [HttpGet]   
        public IActionResult GetInvoice()
        {
            var username = HttpContext.User.Identity.Name;
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return Ok($" Stock işlemi > UserName:{username}-UserId{userIdClaim.Value}");
        }
    }
}
