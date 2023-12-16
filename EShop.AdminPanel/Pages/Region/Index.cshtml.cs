using EShop.AdminPanel.Services;
using EShop.LogService.Repository;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.AdminPanel.Pages.Region
{
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

        public Result<IEnumerable<RegionViewModel>> Regions { get; set; }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, string? country = null, int take = 10, int skip = 0)
        {
            var list = await _regionRepository.GetPaginatedResult(title, country, take, skip);

            return new PartialViewResult
            {
                ViewName = "_RegionList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<RegionViewModel>>(ViewData, list.Data)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Guid id)
        {
            if (id == new Guid())
                return new PartialViewResult
                {
                    ViewName = "_RegionForm",
                    ViewData = new ViewDataDictionary<RegionViewModel>(ViewData, new RegionViewModel())
                };
            else
            {
                var region = await _regionRepository.GetByIdAsync(id);
                return new PartialViewResult
                {
                    ViewName = "_RegionForm",
                    ViewData = new ViewDataDictionary<RegionViewModel>(ViewData, region.Data)
                };
            }
        }
        public async Task<JsonResult> OnGetCreateOrEditAsync(Guid id)
        {
            if (id == new Guid())
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_RegionForm", new RegionViewModel()) });
            else
            {
                var thisRegion = await _regionRepository.GetByIdAsync(id);
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_RegionForm", thisRegion) });
            }
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Guid? id, RegionViewModel region)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                region.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (id == null || id == new Guid())
                {
                    await _regionRepository.AddAsync(region);
                }
                else
                {
                    await _regionRepository.Update(region);
                }
                return await GetRegions(null);
            }
            else
            {
                var html = await _renderService.ToStringAsync("_RegionForm", region);
                return new JsonResult(new { isValid = false, html = html });
            }
        }
        public async Task<JsonResult> OnGetSearchAsync(string title, string country)
        {
            return await GetRegions(new RegionViewModel { Title = title, Country = country});
        }
        public async Task<JsonResult> OnPostDeleteAsync(Guid id)
        {
            await _regionRepository.Delete(id);
            return await GetRegions(null);
        }

        private async Task<JsonResult> GetRegions(RegionViewModel? region)
        {
            if (region == null)
                Regions = await _regionRepository.GetAllAsync();
            else
                Regions = await _regionRepository.GetAllAsync(m => m.Title.Contains(region.Title) && m.Country.Contains(region.Country));

            var html = await _renderService.ToStringAsync("_RegionList", Regions.Data);
            return new JsonResult(new { isValid = true, html = html });
        }
        private async Task<PaginatedViewModel<RegionViewModel>> GetRegions(string? title = null, string? country = null, int take = 10, int skip = 0)
        {
            var totalCount = new SqlParameter("@TotalCount", 0);
            totalCount.Direction = System.Data.ParameterDirection.Output;
            var sparam = new SqlParameter[] {
                new SqlParameter("@Title", title),
                new SqlParameter("@Country", country),
                new SqlParameter("@Take", country),
                new SqlParameter("@Skip", country),
                totalCount
            };

            Regions = await _regionRepository.GetPrecedureAsync<RegionViewModel>("Region_Get", sparam);

            return new PaginatedViewModel<RegionViewModel>
            {
                Data = Regions.Data,
                Take = take,
                Skip = skip,
                TotalCount = Convert.ToInt32(totalCount.Value)
            };
        }
    }
}
