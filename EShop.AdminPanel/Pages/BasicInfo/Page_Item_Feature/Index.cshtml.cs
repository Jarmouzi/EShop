using EShop.AdminPanel.Services;
using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.LogService.Repository;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;

namespace EShop.AdminPanel.Pages.BasicInfo.Page_Item_Feature
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IPage_Item_FeatureRepository _page_item_featureRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IPage_Item_FeatureRepository page_item_featureRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _page_item_featureRepository = page_item_featureRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<Page_Item_FeatureViewModel>();
            try
            {
                var list = await _page_item_featureRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Feature OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_Page_Item_FeatureList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<Page_Item_FeatureViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Guid id)
        {
            try
            {
                if (id == new Guid())
                    return new PartialViewResult
                    {
                        ViewName = "_Page_Item_FeatureForm",
                        ViewData = new ViewDataDictionary<Page_Item_FeatureViewModel>(ViewData, new Page_Item_FeatureViewModel())
                    };
                else
                {
                    var page_item_feature = await _page_item_featureRepository.GetByIdAsync(id);
                    return new PartialViewResult
                    {
                        ViewName = "_Page_Item_FeatureForm",
                        ViewData = new ViewDataDictionary<Page_Item_FeatureViewModel>(ViewData, page_item_feature.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Feature OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_Page_Item_FeatureForm",
                ViewData = new ViewDataDictionary<Page_Item_FeatureViewModel>(ViewData, new Page_Item_FeatureViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Guid id)
        {
            try
            {
                if (id == new Guid())
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Page_Item_FeatureForm", new Page_Item_FeatureViewModel()) });
                else
                {
                    var thisPage_Item_Feature = await _page_item_featureRepository.GetByIdAsync(id);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Page_Item_FeatureForm", thisPage_Item_Feature) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Feature OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Guid? id, Page_Item_FeatureViewModel page_item_feature)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    page_item_feature.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Guid())
                    {
                        await _page_item_featureRepository.AddAsync(page_item_feature);
                    }
                    else
                    {
                        await _page_item_featureRepository.UpdateAsync(page_item_feature);
                    }
                    return await GetPage_Item_Features();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_Page_Item_FeatureForm", page_item_feature);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Feature OnPostCreateOrEditAsync: " + ex.Message, page_item_feature);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await _page_item_featureRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Feature OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetPage_Item_Features();
        }

        private async Task<JsonResult> GetPage_Item_Features()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _page_item_featureRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_Page_Item_FeatureList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Feature GetPage_Item_Features: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
