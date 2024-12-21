using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.LogService.Repository;
using EShop.Model;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EShop.AdminPanel.Pages.Category1
{
    //[AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IRepository<Model.Category, CategoryViewModel> _categoryRepository;

        #region Properties 
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        [BindProperty]
        public CategoryViewModel _Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public Int64 CategoryId { get; set; }
        public SelectList PrimaryCategories { get; set; }
        public SelectList SecondaryCategories { get; set; }

        #endregion Properties

        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IRepository<Model.Category, CategoryViewModel> categoryRepository)
        {
            _logger = logger;
            _dbLog = dbLog;
            _categoryRepository = categoryRepository;
        }
        public async Task OnGet()
        {
            SetSelectLists().Wait();
        }

        private async Task SetSelectLists()
        {
            var result = await _categoryRepository.GetAllAsync(m => m.Level == 1 && m.Confirmed == true);
            Categories = result;
            PrimaryCategories = new SelectList(result, "Id", "Title");
            PrimaryCategories.Prepend(new SelectListItem("انتخاب نمایید", null));

            //result = await _categoryRepository.GetAllAsync(m => m.Level == 2 && m.Confirmed == true);
            //SecondaryCategories = new SelectList(new List<CategoryViewModel>(), "Id", "Title");
            //SecondaryCategories.Prepend(new SelectListItem("انتخاب نمایید", null));
        }
        public async Task<JsonResult> OnGetSubCategories()
        {
            var result = await _categoryRepository.GetAllAsync(m => m.Confirmed == true && m.ParentId == CategoryId);
            return new JsonResult(result);
        }
        public async Task<JsonResult> OnGetDeleteCategory()
        {
            var result = await _categoryRepository.DeleteAsync(CategoryId);
            return new JsonResult(result);
        }
        public async Task<JsonResult> OnGetUpdateCategory()
        {
            var result = await _categoryRepository.GetByIdAsync(CategoryId);

            return new JsonResult(result);
        }

    }
}
