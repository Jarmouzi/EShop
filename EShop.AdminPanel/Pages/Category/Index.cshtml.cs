using EShop.AdminPanel.Services;
using EShop.LogService.Repository;
using EShop.Model;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.AdminPanel.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , ICategoryRepository categoryRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _categoryRepository = categoryRepository;
            _renderService = renderService;
        }

        public Result<IEnumerable<CategoryViewModel>> Categorys { get; set; }
        public void OnGet()
        {
        }
        private async Task<SelectList> SetSelectLists(Guid? parentId)
        {
            var result = await _categoryRepository.GetAllAsync(m => m.Level == 1 && m.Confirmed == true);

            var PrimaryCategories = new SelectList(result.Data, "Id", "Title");
            PrimaryCategories.Prepend(new SelectListItem("انتخاب نمایید", null));

            return PrimaryCategories;
        }
        public async Task<JsonResult> OnGetSubCategories(Guid? parentId)
        {
            var result = await _categoryRepository.GetAllAsync(m => m.Confirmed == true && m.ParentId == parentId);
            return new JsonResult(result.Data);
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(Guid? level1Id = null, Guid? level2Id = null, int take = 10, int skip = 0)
        {
            var list = await _categoryRepository.GetPaginatedResult(level1Id, level2Id, take, skip);

            return new PartialViewResult
            {
                ViewName = "_CategoryList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<CategoryViewModel>>(ViewData, list.Data)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Guid id)
        {
            var result = await _categoryRepository.GetAllAsync(m => m.Level == 1 && m.Confirmed == true);

            var PrimaryCategories = new SelectList(result.Data, "Id", "Title");
            PrimaryCategories.Prepend(new SelectListItem("انتخاب نمایید", null));

            var SecondaryCategories = new SelectList(new List<CategoryViewModel>(), "Id", "Title");
            SecondaryCategories.Prepend(new SelectListItem("انتخاب نمایید", null));

            if (id == new Guid())
            {
                return new PartialViewResult
                {
                    ViewName = "_CategoryForm",
                    ViewData = new ViewDataDictionary<CategoryViewModel>(ViewData, new CategoryViewModel
                    {
                        PrimaryCategories = PrimaryCategories,
                    })
                };
            }
            else
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                var level = category.Data.Level;

                switch (level)
                {
                    case 1:
                        PrimaryCategories.Select(m => m.Value == null);
                        break;
                    case 2:
                        PrimaryCategories.Select(m => m.Value == category.Data.ParentId.ToString());
                        result = await _categoryRepository.GetAllAsync(m => m.Level == 2 && m.Confirmed == true && m.ParentId == category.Data.ParentId);
                        SecondaryCategories = new SelectList(result.Data, "Id", "Title");
                        SecondaryCategories.Prepend(new SelectListItem("انتخاب نمایید", null));
                        SecondaryCategories.Select(m => level == 1 && m.Value == category.Data.Id.ToString());
                        break;
                    case 3:
                        var grandParentId = result.Data.First().ParentId;
                        PrimaryCategories.Select(m => m.Value == grandParentId.ToString());
                        result = await _categoryRepository.GetAllAsync(m => m.Level == 2 && m.Confirmed == true && m.ParentId == grandParentId);

                        SecondaryCategories = new SelectList(result.Data, "Id", "Title");
                        SecondaryCategories.Prepend(new SelectListItem("انتخاب نمایید", null));
                        SecondaryCategories.Select(m => level == 1 && m.Value == category.Data.Id.ToString());
                        break;

                    default:
                        break;
                }

                category.Data.PrimaryCategories = PrimaryCategories;
                category.Data.SecondaryCategories = SecondaryCategories;
                return new PartialViewResult
                {
                    ViewName = "_CategoryForm",
                    ViewData = new ViewDataDictionary<CategoryViewModel>(ViewData, category.Data)
                };
            }
        }
        public async Task<JsonResult> OnGetCreateOrEditAsync(Guid id)
        {
            if (id == new Guid())
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CategoryForm", new CategoryViewModel()) });
            else
            {
                var thisCategory = await _categoryRepository.GetByIdAsync(id);
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CategoryForm", thisCategory) });
            }
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Guid? id, CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                category.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (category.ParentId == null && category.GrandParentId != null) 
                    category.ParentId = category.GrandParentId;

                if (id == null || id == new Guid())
                {
                    await _categoryRepository.AddAsync(category);
                }
                else
                {
                    await _categoryRepository.Update(category);
                }
                return await GetCategories();
            }
            else
            {
                var html = await _renderService.ToStringAsync("_CategoryForm", category);
                return new JsonResult(new { isValid = false, html = html });
            }
        }
        public async Task<JsonResult> OnPostDeleteAsync(Guid id)
        {
            await _categoryRepository.Delete(id);
            return await GetCategories();
        }

        private async Task<JsonResult> GetCategories(Guid? level1Id = null, Guid? level2Id = null, int take = 10, int skip = 0)
        {
            var list = await _categoryRepository.GetPaginatedResult(level1Id, level2Id, take, skip);

            var html = await _renderService.ToStringAsync("_CategoryList", list.Data);
            return new JsonResult(new { isValid = true, html = html });
        }
    }
}
