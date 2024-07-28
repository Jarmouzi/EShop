using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.AdminPanel.Pages.Supplier
{
    public class ProfileModel : PageModel
    {
        [BindProperty] public SupplierViewModel Supplier { get; set; }
        public void OnGet()
        {
        }
    }
}
