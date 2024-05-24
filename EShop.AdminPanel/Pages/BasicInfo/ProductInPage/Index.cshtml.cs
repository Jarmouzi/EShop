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

namespace EShop.AdminPanel.Pages.BasicInfo.ProductInPage
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProductInPageRepository _productinpageRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProductInPageRepository productinpageRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _productinpageRepository = productinpageRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<ProductInPageViewModel>();
            try
            {
                var list = await _productinpageRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductInPage OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_ProductInPageList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<ProductInPageViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new PartialViewResult
                    {
                        ViewName = "_ProductInPageForm",
                        ViewData = new ViewDataDictionary<ProductInPageViewModel>(ViewData, new ProductInPageViewModel())
                    };
                else
                {
                    var productinpage = await _productinpageRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_ProductInPageForm",
                        ViewData = new ViewDataDictionary<ProductInPageViewModel>(ViewData, productinpage.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductInPage OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_ProductInPageForm",
                ViewData = new ViewDataDictionary<ProductInPageViewModel>(ViewData, new ProductInPageViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ProductInPageForm", new ProductInPageViewModel()) });
                else
                {
                    var thisProductInPage = await _productinpageRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ProductInPageForm", thisProductInPage) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductInPage OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, ProductInPageViewModel productinpage)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    productinpage.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue)
                    {
                        await _productinpageRepository.AddAsync(productinpage);
                    }
                    else
                    {
                        await _productinpageRepository.UpdateAsync(productinpage);
                    }
                    return await GetProductInPages();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_ProductInPageForm", productinpage);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductInPage OnPostCreateOrEditAsync: " + ex.Message, productinpage);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _productinpageRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductInPage OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetProductInPages();
        }

        private async Task<JsonResult> GetProductInPages()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _productinpageRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_ProductInPageList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductInPage GetProductInPages: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
