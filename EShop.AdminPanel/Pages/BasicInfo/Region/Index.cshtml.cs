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

namespace EShop.AdminPanel.Pages.BasicInfo.Region
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IRegionRepository _regionRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IRegionRepository regionRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _regionRepository = regionRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, string? country = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<RegionViewModel>();
            try
            {
                var list = await _regionRepository.GetPaginatedResult(title, country, take, skip);

                result = list;
            }
            catch (Exception ex)
            {
                _logger.LogError("Region OnGetViewAllPartial: " + ex.Message, [title, country, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_RegionList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<RegionViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_RegionForm",
                        ViewData = new ViewDataDictionary<RegionViewModel>(ViewData, new RegionViewModel())
                    };
                else
                {
                    var region = await _regionRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_RegionForm",
                        ViewData = new ViewDataDictionary<RegionViewModel>(ViewData, region)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Region OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_RegionForm",
                ViewData = new ViewDataDictionary<RegionViewModel>(ViewData, new RegionViewModel())
            };
        }
        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_RegionForm", new RegionViewModel()) });
                else
                {
                    var thisRegion = await _regionRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_RegionForm", thisRegion) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Region OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, RegionViewModel region)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    region.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _regionRepository.AddAsync(region);
                    }
                    else
                    {
                        await _regionRepository.UpdateAsync(region);
                    }
                    return await GetRegions();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_RegionForm", region);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Region OnPostCreateOrEditAsync: " + ex.Message, region);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _regionRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Region OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetRegions();
        }

        private async Task<JsonResult> GetRegions()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _regionRepository.GetPaginatedResult(null, null, 10, 0);

                

                html = await _renderService.ToStringAsync("_RegionList", list);
            }
            catch (Exception ex)
            {
                _logger.LogError("Region GetRegions: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
