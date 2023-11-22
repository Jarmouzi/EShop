using EShop.IdentityService.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.AdminPanel.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private IAuthService _authService;

        public LogoutModel(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _authService.Logout();
            return RedirectToPage("/Index");
        }
    }
}
