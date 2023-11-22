using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.AdminPanel.Pages
{
    //[Authorize]
    [Authorize(Roles ="Admin")]
    //[Authorize(Roles ="admin1")]
    public class OnlyAdminPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
