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
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
                    return Ok(false);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
