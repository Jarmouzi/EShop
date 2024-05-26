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

namespace EShop.AdminPanel.Pages.BasicInfo.ProductSeo
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProductSeoRepository _productseoRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProductSeoRepository productseoRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _productseoRepository = productseoRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<ProductSeoViewModel>();
            try
            {
                var list = await _productseoRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductSeo OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_ProductSeoList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<ProductSeoViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_ProductSeoForm",
                        ViewData = new ViewDataDictionary<ProductSeoViewModel>(ViewData, new ProductSeoViewModel())
                    };
                else
                {
                    var productseo = await _productseoRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_ProductSeoForm",
                        ViewData = new ViewDataDictionary<ProductSeoViewModel>(ViewData, productseo.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductSeo OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_ProductSeoForm",
                ViewData = new ViewDataDictionary<ProductSeoViewModel>(ViewData, new ProductSeoViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ProductSeoForm", new ProductSeoViewModel()) });
                else
                {
                    var thisProductSeo = await _productseoRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ProductSeoForm", thisProductSeo) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductSeo OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, ProductSeoViewModel productseo)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    productseo.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Int64())
                    {
                        await _productseoRepository.AddAsync(productseo);
                    }
                    else
                    {
                        await _productseoRepository.UpdateAsync(productseo);
                    }
                    return await GetProductSeos();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_ProductSeoForm", productseo);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductSeo OnPostCreateOrEditAsync: " + ex.Message, productseo);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _productseoRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductSeo OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetProductSeos();
        }

        private async Task<JsonResult> GetProductSeos()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _productseoRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_ProductSeoList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductSeo GetProductSeos: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
