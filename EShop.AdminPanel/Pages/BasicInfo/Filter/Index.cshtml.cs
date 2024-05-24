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

namespace EShop.AdminPanel.Pages.BasicInfo.Filter
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IFilterRepository _filterRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IFilterRepository filterRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _filterRepository = filterRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<FilterViewModel>();
            try
            {
                var list = await _filterRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Filter OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_FilterList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<FilterViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new PartialViewResult
                    {
                        ViewName = "_FilterForm",
                        ViewData = new ViewDataDictionary<FilterViewModel>(ViewData, new FilterViewModel())
                    };
                else
                {
                    var filter = await _filterRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_FilterForm",
                        ViewData = new ViewDataDictionary<FilterViewModel>(ViewData, filter.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Filter OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_FilterForm",
                ViewData = new ViewDataDictionary<FilterViewModel>(ViewData, new FilterViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_FilterForm", new FilterViewModel()) });
                else
                {
                    var thisFilter = await _filterRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_FilterForm", thisFilter) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Filter OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, FilterViewModel filter)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    filter.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue)
                    {
                        await _filterRepository.AddAsync(filter);
                    }
                    else
                    {
                        await _filterRepository.UpdateAsync(filter);
                    }
                    return await GetFilters();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_FilterForm", filter);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Filter OnPostCreateOrEditAsync: " + ex.Message, filter);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _filterRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Filter OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetFilters();
        }

        private async Task<JsonResult> GetFilters()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _filterRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_FilterList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Filter GetFilters: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
