using EShop.AdminPanel.Services;
using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.LogService.Repository;
using EShop.Model;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;

namespace EShop.AdminPanel.Pages.Category
{
    [AuthorizePage]
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
        public async Task<PartialViewResult> OnGetViewAllPartial(Int64? parentId = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<CategoryViewModel>();
            try
            {
                var list = await _categoryRepository.GetPaginatedResult(parentId, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Category OnGetViewAllPartial: " + ex.Message, [parentId, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_CategoryList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<CategoryViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                var result = await _categoryRepository.GetAllAsync();
                var list = result.Data.OrderBy(m => m.ParentOrder + m.DisplayOrder).ToList();

                if (!id.HasValue)
                    return new PartialViewResult
                    {
                        ViewName = "_CategoryForm",
                        ViewData = new ViewDataDictionary<CategoryViewModel>(ViewData, new CategoryViewModel
                        {
                            Categories = list,
                        })
                    };
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
            catch (Exception ex)
            {
                _logger.LogError("Category OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_CategoryForm",
                ViewData = new ViewDataDictionary<CategoryViewModel>(ViewData, new CategoryViewModel())
            };
        }
        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CategoryForm", new CategoryViewModel()) });
                else
                {
                    var thisCategory = await _categoryRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CategoryForm", thisCategory) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Category OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, CategoryViewModel category)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                byte level = 1;
                if (category.ParentId.HasValue)
                {
                    var parent = await _categoryRepository.GetByIdAsync(category.ParentId.Value);

                    if(parent.Data != null)
                        level = ++parent.Data.Level;
                }
                category.Level = level;

                if (ModelState.IsValid)
                {
                    category.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue)
                    {
                        await _categoryRepository.AddAsync(category);
                    }
                    else
                    {
                        await _categoryRepository.UpdateAsync(category);
                    }
                    return await GetCategorys();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_CategoryForm", category);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Category OnPostCreateOrEditAsync: " + ex.Message, category);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _categoryRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Category OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetCategorys();
        }
        public async Task<JsonResult> OnPosChangeOrderAsync(Int64 id, int order)
        {
            try
            {
                await _categoryRepository.ChangeDisplayOrder(id, order);
            }
            catch (Exception ex)
            {
                _logger.LogError("Category OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetCategorys();
        }

        private async Task<JsonResult> GetCategorys()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _categoryRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_CategoryList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Category GetCategorys: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
