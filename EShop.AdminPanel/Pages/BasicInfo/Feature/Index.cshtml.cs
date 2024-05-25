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

namespace EShop.AdminPanel.Pages.BasicInfo.Feature
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IFeatureRepository _featureRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IFeatureRepository featureRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _featureRepository = featureRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<FeatureViewModel>();
            try
            {
                var list = await _featureRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Feature OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_FeatureList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<FeatureViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_FeatureForm",
                        ViewData = new ViewDataDictionary<FeatureViewModel>(ViewData, new FeatureViewModel())
                    };
                else
                {
                    var feature = await _featureRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_FeatureForm",
                        ViewData = new ViewDataDictionary<FeatureViewModel>(ViewData, feature.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Feature OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_FeatureForm",
                ViewData = new ViewDataDictionary<FeatureViewModel>(ViewData, new FeatureViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_FeatureForm", new FeatureViewModel()) });
                else
                {
                    var thisFeature = await _featureRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_FeatureForm", thisFeature) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Feature OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, FeatureViewModel feature)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    feature.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _featureRepository.AddAsync(feature);
                    }
                    else
                    {
                        await _featureRepository.UpdateAsync(feature);
                    }
                    return await GetFeatures();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_FeatureForm", feature);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Feature OnPostCreateOrEditAsync: " + ex.Message, feature);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _featureRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Feature OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetFeatures();
        }

        private async Task<JsonResult> GetFeatures()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _featureRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_FeatureList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Feature GetFeatures: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
