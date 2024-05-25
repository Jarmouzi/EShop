using EShop.IdentityService.Identity;
using EShop.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EShop.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtConfiguration _config;

        public AuthController(
            IAuthService authService,
            IOptions<JwtConfiguration> config
            )
        {
            _authService = authService;
            _config = config.Value;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm]LoginUser credentials)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException("Login credentials");
            }
            var userId = await _authService.Login(credentials);
            if (userId.HasValue)
            {
                //var visitLogId = await _logRepository.AddVisitLogAsync(new LogService.Model.VisitLog
                //{
                //    Date = DateTime.Now,
                //    ExpireDate = DateTime.Now.AddHours(24),
                //    IP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                //    Language = Thread.CurrentThread.CurrentCulture.Name,
                //    UserId = userId.Value,
                //    DeviceInfo = Request.Headers["User-Agent"].ToString()
                //});

                // check IP region


                return Ok(
                    new
                    {
                        token = await _authService.GenerateTokenString(credentials.Username, _config)
                    }
                );
            }

            return BadRequest();
        }

        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateToken(string email)
        {
            var token = await _authService.GenerateOTPToken(email);

            //send email 


            return Ok(token);

            //return BadRequest();
        }

        [HttpPost("VerifyToken")]
        public async Task<IActionResult> VerifyToken(string email, string token)
        {
            var user = await _authService.VerifyOTPToken(email, token);

            if(user != null)

                return Ok(
                    new
                    {
                        token = await _authService.GenerateTokenString(user, _config)
                    });

            return BadRequest();
        }
    }
}
