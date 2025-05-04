using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("IsAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            try
            {
                return Ok(User.Identity != null && User.Identity.IsAuthenticated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
