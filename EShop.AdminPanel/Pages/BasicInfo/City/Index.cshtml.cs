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

namespace EShop.AdminPanel.Pages.BasicInfo.City
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly ICityRepository _cityRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , ICityRepository cityRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _cityRepository = cityRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<CityViewModel>();
            try
            {
                var list = await _cityRepository.GetPaginatedResult(title, take, skip);

                result = list;
            }
            catch (Exception ex)
            {
                _logger.LogError("City OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_CityList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<CityViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_CityForm",
                        ViewData = new ViewDataDictionary<CityViewModel>(ViewData, new CityViewModel())
                    };
                else
                {
                    var city = await _cityRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_CityForm",
                        ViewData = new ViewDataDictionary<CityViewModel>(ViewData, city)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("City OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_CityForm",
                ViewData = new ViewDataDictionary<CityViewModel>(ViewData, new CityViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CityForm", new CityViewModel()) });
                else
                {
                    var thisCity = await _cityRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CityForm", thisCity) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("City OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, CityViewModel city)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    city.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _cityRepository.AddAsync(city);
                    }
                    else
                    {
                        await _cityRepository.UpdateAsync(city);
                    }
                    return await GetCitys();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_CityForm", city);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("City OnPostCreateOrEditAsync: " + ex.Message, city);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _cityRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("City OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetCitys();
        }

        private async Task<JsonResult> GetCitys()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _cityRepository.GetPaginatedResult(null, 10, 0);

                

                html = await _renderService.ToStringAsync("_CityList", list);
            }
            catch (Exception ex)
            {
                _logger.LogError("City GetCitys: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
