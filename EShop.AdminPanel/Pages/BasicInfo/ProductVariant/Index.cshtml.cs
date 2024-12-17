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

namespace EShop.AdminPanel.Pages.BasicInfo.ProductVariant
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProductVariantRepository _productvariantRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProductVariantRepository productvariantRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _productvariantRepository = productvariantRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<ProductVariantViewModel>();
            try
            {
                var list = await _productvariantRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_ProductVariantList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<ProductVariantViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_ProductVariantForm",
                        ViewData = new ViewDataDictionary<ProductVariantViewModel>(ViewData, new ProductVariantViewModel())
                    };
                else
                {
                    var productvariant = await _productvariantRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_ProductVariantForm",
                        ViewData = new ViewDataDictionary<ProductVariantViewModel>(ViewData, productvariant.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_ProductVariantForm",
                ViewData = new ViewDataDictionary<ProductVariantViewModel>(ViewData, new ProductVariantViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ProductVariantForm", new ProductVariantViewModel()) });
                else
                {
                    var thisProductVariant = await _productvariantRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ProductVariantForm", thisProductVariant) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, ProductVariantViewModel productvariant)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    productvariant.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _productvariantRepository.AddAsync(productvariant);
                    }
                    else
                    {
                        await _productvariantRepository.UpdateAsync(productvariant);
                    }
                    return await GetProductVariants();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_ProductVariantForm", productvariant);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant OnPostCreateOrEditAsync: " + ex.Message, productvariant);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _productvariantRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetProductVariants();
        }

        private async Task<JsonResult> GetProductVariants()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _productvariantRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_ProductVariantList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant GetProductVariants: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
