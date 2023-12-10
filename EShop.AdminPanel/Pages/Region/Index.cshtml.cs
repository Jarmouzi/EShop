using EShop.AdminPanel.Services;
using EShop.LogService.Repository;
using EShop.Service.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShop.AdminPanel.Pages.Region
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IRepository<Model.Region, RegionViewModel> _regionRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IRepository<Model.Region, RegionViewModel> regionRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _regionRepository = regionRepository;
            _renderService = renderService;
        }

        public IEnumerable<RegionViewModel> Regions { get; set; }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial()
        {
            var result = await _regionRepository.GetAllAsync();
            Regions = result.Data;
            return new PartialViewResult
            {
                ViewName = "_RegionList",
                ViewData = new ViewDataDictionary<IEnumerable<RegionViewModel>>(ViewData, Regions)
            };
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
        public async Task<JsonResult> OnPostCreateOrEditAsync(int id, RegionViewModel region)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    await _regionRepository.AddAsync(region);
                }
                else
                {
                    await _regionRepository.Update(region);
                }
                var result = await _regionRepository.GetAllAsync();
                Regions = result.Data;
                var html = await _renderService.ToStringAsync("_RegionList", Regions);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var html = await _renderService.ToStringAsync("_RegionForm", region);
                return new JsonResult(new { isValid = false, html = html });
            }
        }
        public async Task<JsonResult> OnPostDeleteAsync(Guid id)
        {
            await _regionRepository.Delete(id);
            var result = await _regionRepository.GetAllAsync();
            Regions = result.Data;
            var html = await _renderService.ToStringAsync("_RegionList", Regions);
            return new JsonResult(new { isValid = true, html = html });
        }
    }
}
