using EShop.AdminPanel.Services;
using EShop.LogService.Repository;
using EShop.Model;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.AdminPanel.Pages.Category2
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

        public void OnGet()
        {
        }
        //private async Task<SelectList> SetSelectLists(Int64? parentId)
        //{
        //    var result = await _categoryRepository.GetAllAsync();// m => m.Level == 1);

        //    var PrimaryCategories = new SelectList(result.Data, "Id", "Title", null, "ParentId");

        //    PrimaryCategories.Prepend(new SelectListItem("انتخاب نمایید", null));

        //    return PrimaryCategories;
        //}


        public async Task<JsonResult> OnGetSubCategories(Int64? parentId)
        {
            var result = await _categoryRepository.GetAllAsync();// m => m.ParentId == parentId);
            return new JsonResult(result.Data);
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(Int64? parentId = null, int take = 10, int skip = 0)
        {
            var list = await _categoryRepository.GetPaginatedResult(parentId, take, skip);

            return new PartialViewResult
            {
                ViewName = "_CategoryList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<CategoryViewModel>>(ViewData, list.Data)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            var result = await _categoryRepository.GetAllAsync();
            var list = result.Data.OrderBy(m => m.ParentOrder + m.DisplayOrder).ToList();

            if (!id.HasValue || id == 0)
            {
                return new PartialViewResult
                {
                    ViewName = "_CategoryForm",
                    ViewData = new ViewDataDictionary<CategoryViewModel>(ViewData, new CategoryViewModel
                    {
                        Categories = list,
                    })
                };
            }
            else
            {
                var category = await _categoryRepository.GetByIdAsync(id.Value);
                category.Data.Categories = list;

                return new PartialViewResult
                {
                    ViewName = "_CategoryForm",
                    ViewData = new ViewDataDictionary<CategoryViewModel>(ViewData, category.Data)
                };
            }
        }
        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            if (!id.HasValue || id == 0)
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CategoryForm", new CategoryViewModel()) });
            else
            {
                var thisCategory = await _categoryRepository.GetByIdAsync(id.Value);
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CategoryForm", thisCategory) });
            }
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                category.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (category.ParentId == null)
                {
                    if (category.GrandParentId != null)
                    {
                        category.ParentId = category.GrandParentId;
                        category.Level = 2;
                    }
                    else
                        category.Level = 1;
                }
                else
                    category.Level = 3;

                if (!id.HasValue || id == 0)
                {
                    await _categoryRepository.AddAsync(category);
                }
                else
                {
                    await _categoryRepository.UpdateAsync(category);
                }
                return await GetCategories();
            }
            else
            {
                var html = await _renderService.ToStringAsync("_CategoryForm", category);
                return new JsonResult(new { isValid = false, html = html });
            }
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            await _categoryRepository.DeleteAsync(id);
            return await GetCategories();
        }

        private async Task<JsonResult> GetCategories(Int64? parentId = null, int take = 10, int skip = 0)
        {
            var list = await _categoryRepository.GetPaginatedResult(parentId, take, skip);

            var html = await _renderService.ToStringAsync("_CategoryList", list.Data);
            return new JsonResult(new { isValid = true, html = html });
        }
    }
}
