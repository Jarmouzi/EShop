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
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

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

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    var userId = await _authService.Login(UserCredential);
        //    if ( userId.HasValue )
        //    {
        //        var visitLogId = await _logRepository.AddVisitLogAsync(new LogService.Model.VisitLog
        //        {
        //            Date = DateTime.Now,
        //            ExpireDate = DateTime.Now.AddHours(24),
        //            IP = HttpContext.Connection.RemoteIpAddress?.ToString(),
        //            Language = Thread.CurrentThread.CurrentCulture.Name,
        //            UserId = userId.Value,
        //            DeviceInfo = Request.Headers["User-Agent"].ToString()
        //        });
        //        //Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))


        //        if (visitLogId != null)
        //        {
        //            await _authService.AddUpdateClaim(UserCredential.Username, new Claim("VLI", visitLogId.ToString()));
        //        }

        //        await _authService.GenerateCookieAuthentication(UserCredential.Username);
        //        return RedirectToPage("/Index");
        //    }
        //    return Page();
        //}

        public async Task<IActionResult> OnPostAsync()
        {

            var ServerIP = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("EShopSettings")["AuthServer"];
            using (var client = new HttpClient { BaseAddress = new Uri(ServerIP) })
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                
                var credentialContent = new MultipartFormDataContent();

                // Add string fields
                credentialContent.Add(new StringContent(UserCredential.Username), "Username");
                credentialContent.Add(new StringContent(UserCredential.Password), "Password");

                try
                {
                    var result = client.PostAsync("/api/auth/login/", credentialContent).Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var auth_result = result.Content.ReadAsStringAsync().Result;
                        var jsonObject = (JObject)JsonConvert.DeserializeObject(auth_result);
                        await _authService.AddUpdateClaim(UserCredential.Username, new Claim("token", jsonObject["token"].ToString()));

                        var visitLogId = await _logRepository.AddVisitLogAsync(new LogService.Model.VisitLog
                        {
                            Date = DateTime.Now,
                            ExpireDate = DateTime.Now.AddHours(24),
                            IP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                            Language = Thread.CurrentThread.CurrentCulture.Name,
                            Username = UserCredential.Username,
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
                }
                catch (Exception e)
                {
                    var x = "\n Insert To " + ServerIP + " raised error: " + e.Message;
                }
                return Page();
            }
        }
    }
}
