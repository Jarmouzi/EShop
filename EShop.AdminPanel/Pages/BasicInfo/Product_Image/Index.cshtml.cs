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

namespace EShop.AdminPanel.Pages.BasicInfo.Product_Image
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProduct_ImageRepository _product_imageRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProduct_ImageRepository product_imageRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _product_imageRepository = product_imageRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<Product_ImageViewModel>();
            try
            {
                var list = await _product_imageRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Image OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_Product_ImageList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<Product_ImageViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_Product_ImageForm",
                        ViewData = new ViewDataDictionary<Product_ImageViewModel>(ViewData, new Product_ImageViewModel())
                    };
                else
                {
                    var product_image = await _product_imageRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_Product_ImageForm",
                        ViewData = new ViewDataDictionary<Product_ImageViewModel>(ViewData, product_image.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Image OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_Product_ImageForm",
                ViewData = new ViewDataDictionary<Product_ImageViewModel>(ViewData, new Product_ImageViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_ImageForm", new Product_ImageViewModel()) });
                else
                {
                    var thisProduct_Image = await _product_imageRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_ImageForm", thisProduct_Image) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Image OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, Product_ImageViewModel product_image)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    product_image.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Int64())
                    {
                        await _product_imageRepository.AddAsync(product_image);
                    }
                    else
                    {
                        await _product_imageRepository.UpdateAsync(product_image);
                    }
                    return await GetProduct_Images();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_Product_ImageForm", product_image);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Image OnPostCreateOrEditAsync: " + ex.Message, product_image);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _product_imageRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Image OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetProduct_Images();
        }

        private async Task<JsonResult> GetProduct_Images()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _product_imageRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_Product_ImageList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Image GetProduct_Images: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
