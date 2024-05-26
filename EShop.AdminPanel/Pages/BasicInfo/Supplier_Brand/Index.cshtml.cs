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

namespace EShop.AdminPanel.Pages.BasicInfo.Supplier_Brand
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly ISupplier_BrandRepository _supplier_brandRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , ISupplier_BrandRepository supplier_brandRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _supplier_brandRepository = supplier_brandRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<Supplier_BrandViewModel>();
            try
            {
                var list = await _supplier_brandRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Brand OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_Supplier_BrandList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<Supplier_BrandViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_Supplier_BrandForm",
                        ViewData = new ViewDataDictionary<Supplier_BrandViewModel>(ViewData, new Supplier_BrandViewModel())
                    };
                else
                {
                    var supplier_brand = await _supplier_brandRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_Supplier_BrandForm",
                        ViewData = new ViewDataDictionary<Supplier_BrandViewModel>(ViewData, supplier_brand.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Brand OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_Supplier_BrandForm",
                ViewData = new ViewDataDictionary<Supplier_BrandViewModel>(ViewData, new Supplier_BrandViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Supplier_BrandForm", new Supplier_BrandViewModel()) });
                else
                {
                    var thisSupplier_Brand = await _supplier_brandRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Supplier_BrandForm", thisSupplier_Brand) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Brand OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, Supplier_BrandViewModel supplier_brand)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    supplier_brand.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Int64())
                    {
                        await _supplier_brandRepository.AddAsync(supplier_brand);
                    }
                    else
                    {
                        await _supplier_brandRepository.UpdateAsync(supplier_brand);
                    }
                    return await GetSupplier_Brands();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_Supplier_BrandForm", supplier_brand);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Brand OnPostCreateOrEditAsync: " + ex.Message, supplier_brand);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _supplier_brandRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Brand OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetSupplier_Brands();
        }

        private async Task<JsonResult> GetSupplier_Brands()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _supplier_brandRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_Supplier_BrandList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Brand GetSupplier_Brands: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
