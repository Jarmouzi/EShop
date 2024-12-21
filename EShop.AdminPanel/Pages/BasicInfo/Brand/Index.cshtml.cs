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

namespace EShop.AdminPanel.Pages.BasicInfo.Brand
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IBrandRepository _brandRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IBrandRepository brandRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _brandRepository = brandRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<BrandViewModel>();
            try
            {
                var list = await _brandRepository.GetPaginatedResult(title, take, skip);

                result = list;
            }
            catch (Exception ex)
            {
                _logger.LogError("Brand OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_BrandList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<BrandViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_BrandForm",
                        ViewData = new ViewDataDictionary<BrandViewModel>(ViewData, new BrandViewModel())
                    };
                else
                {
                    var brand = await _brandRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_BrandForm",
                        ViewData = new ViewDataDictionary<BrandViewModel>(ViewData, brand)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Brand OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_BrandForm",
                ViewData = new ViewDataDictionary<BrandViewModel>(ViewData, new BrandViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_BrandForm", new BrandViewModel()) });
                else
                {
                    var thisBrand = await _brandRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_BrandForm", thisBrand) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Brand OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, BrandViewModel brand)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    brand.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _brandRepository.AddAsync(brand);
                    }
                    else
                    {
                        await _brandRepository.UpdateAsync(brand);
                    }
                    return await GetBrands();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_BrandForm", brand);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Brand OnPostCreateOrEditAsync: " + ex.Message, brand);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _brandRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Brand OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetBrands();
        }

        private async Task<JsonResult> GetBrands()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _brandRepository.GetPaginatedResult(null, 10, 0);

                

                html = await _renderService.ToStringAsync("_BrandList", list);
            }
            catch (Exception ex)
            {
                _logger.LogError("Brand GetBrands: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
