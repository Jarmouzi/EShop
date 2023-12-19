using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.AdminPanel.Pages.Category
{
    public class FilterModel : PageModel
    {
        public Guid Id { get; set; }
        public void OnGet(Guid id)
        {
            Id = id;
        }
    }
}
