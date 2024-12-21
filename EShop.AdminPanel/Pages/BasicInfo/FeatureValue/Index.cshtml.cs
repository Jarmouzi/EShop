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

namespace EShop.AdminPanel.Pages.BasicInfo.FeatureValue
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IFeatureValueRepository _featurevalueRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IFeatureValueRepository featurevalueRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _featurevalueRepository = featurevalueRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<FeatureValueViewModel>();
            try
            {
                var list = await _featurevalueRepository.GetPaginatedResult(title, take, skip);

                result = list;
            }
            catch (Exception ex)
            {
                _logger.LogError("FeatureValue OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_FeatureValueList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<FeatureValueViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_FeatureValueForm",
                        ViewData = new ViewDataDictionary<FeatureValueViewModel>(ViewData, new FeatureValueViewModel())
                    };
                else
                {
                    var featurevalue = await _featurevalueRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_FeatureValueForm",
                        ViewData = new ViewDataDictionary<FeatureValueViewModel>(ViewData, featurevalue)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("FeatureValue OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_FeatureValueForm",
                ViewData = new ViewDataDictionary<FeatureValueViewModel>(ViewData, new FeatureValueViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_FeatureValueForm", new FeatureValueViewModel()) });
                else
                {
                    var thisFeatureValue = await _featurevalueRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_FeatureValueForm", thisFeatureValue) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("FeatureValue OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, FeatureValueViewModel featurevalue)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    featurevalue.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _featurevalueRepository.AddAsync(featurevalue);
                    }
                    else
                    {
                        await _featurevalueRepository.UpdateAsync(featurevalue);
                    }
                    return await GetFeatureValues();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_FeatureValueForm", featurevalue);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("FeatureValue OnPostCreateOrEditAsync: " + ex.Message, featurevalue);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _featurevalueRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("FeatureValue OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetFeatureValues();
        }

        private async Task<JsonResult> GetFeatureValues()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _featurevalueRepository.GetPaginatedResult(null, 10, 0);

                

                html = await _renderService.ToStringAsync("_FeatureValueList", list);
            }
            catch (Exception ex)
            {
                _logger.LogError("FeatureValue GetFeatureValues: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
