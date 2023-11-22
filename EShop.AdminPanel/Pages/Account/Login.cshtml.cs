using EShop.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EShop.IdentityService.Identity;
using EShop.LogService.Repository;
using System.Text.RegularExpressions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using EShop.LogService.Model;

namespace EShop.AdminPanel.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ILogRepository _logRepository;

        [BindProperty]
        public LoginUser UserCredential { get; set; }

        public LoginModel(IAuthService authService, ILogRepository logRepository)
        {
            _authService = authService;
            _logRepository = logRepository;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = await _authService.Login(UserCredential);
            if ( userId.HasValue )
            {
                var visitLogId = await _logRepository.AddVisitLogAsync(new LogService.Model.VisitLog
                {
                    Date = DateTime.Now,
                    ExpireDate = DateTime.Now.AddHours(24),
                    IP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Language = Thread.CurrentThread.CurrentCulture.Name,
                    UserId = userId.Value,
                    DeviceInfo = Request.Headers["User-Agent"].ToString()
                });
                //Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))


                if (visitLogId != null)
                {
                    await _authService.AddUpdateClaim(UserCredential.Username, new Claim("VLI", visitLogId.ToString()));
                }

                await _authService.GenerateCookieAuthentication(UserCredential.Username);
                return RedirectToPage("/Index");
            }
            return Page();
        }

    }
}
