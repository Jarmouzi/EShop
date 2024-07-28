using EShop.IdentityService.Identity;
using EShop.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.AdminPanel.Pages.Account
{
    [Authorize]
    public class SupplierRegistrationModel : PageModel
    {
        private IAuthService _authService;

        [BindProperty]
        public LoginUser UserCredential { get; set; }

        public SupplierRegistrationModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (await _authService.RegisterUser(UserCredential))
            {
                return RedirectToPage("/Account/Login");
            }
            return BadRequest();
        }
    }
}
