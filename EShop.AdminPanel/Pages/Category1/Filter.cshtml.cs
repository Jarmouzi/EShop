using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.AdminPanel.Pages.Category
{
    public class FilterModel : PageModel
    {
        public Int64 Id { get; set; }
        public void OnGet(Int64 id)
        {
            Id = id;
        }
    }
}
