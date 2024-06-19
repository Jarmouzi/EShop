using EShop.AdminPanel.Services;
using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.LogService.Repository;
using EShop.Model;
using EShop.Model.TypeSafe;
using EShop.Repository.Implementation;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;

namespace EShop.AdminPanel.Pages.Product.Product_Image
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProductRepository _productRepository;
        private readonly IProduct_OptionRepository _productOptionRepository;
        private readonly IProduct_ImageRepository _product_imageRepository;
        private readonly IRazorRenderService _renderService;

        [BindProperty]
        public Int64? ProductId { get; set; }

        [BindProperty]
        public Int64? Product_OptionId { get; set; }


        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProductRepository productRepository
            , IProduct_OptionRepository productOptionRepository
            , IProduct_ImageRepository product_imageRepository
            , IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _productRepository = productRepository;
            _productOptionRepository = productOptionRepository;
            _product_imageRepository = product_imageRepository;
            _renderService = renderService;
        }
        public void OnGet(Int64? id = null, Int64? oId = null)
        {
            ProductId = id;
            Product_OptionId = oId;
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(Int64? pId, Int64? oId)
        {
            //var result = new PaginatedViewModel<Product_ImageViewModel>();
            var result = new List<Product_ImageViewModel>();
            try
            {
                var list = await _product_imageRepository.GetPaginatedResult(pId, oId);

                result = list.Data.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Image OnGetViewAllPartial: " + ex.Message, [pId, oId]);
            }

            return new PartialViewResult
            {
                ViewName = "_Product_ImageList",
                ViewData = new ViewDataDictionary<List<Product_ImageViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? pId, Int64? oId, Int64? id)
        {
            try
            {
                string productTitle = "", productVariantTitle = "";
                if (pId.HasValue)
                {
                    var product = await _productRepository.GetByIdAsync(pId.Value);
                    productTitle = product.Data.Title;
                }
                //if (ProductVariantId.HasValue)
                //{
                //    var product = await _productVariantRepository.GetByIdAsync(ProductVariantId.Value);
                //    productTitle = product.Data.ProductTitle;
                //}
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_Product_ImageForm",
                        ViewData = new ViewDataDictionary<Product_ImageViewModel>(ViewData, 
                        new Product_ImageViewModel { ProductTitle = productTitle, ProductId = pId??0, Product_OptionId = oId })
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
                //var productVariant = await 
                if (!id.HasValue || id == 0)
                {
                    var product = await _productRepository.GetByIdAsync(ProductId.Value);
                    //var productVariant = await _productVariantRepository.GetByIdAsync(ProductVariantId);

                    return new JsonResult(new
                    {
                        isValid = true,
                        html = await _renderService.ToStringAsync("_Product_ImageForm",
                                new Product_ImageViewModel { ProductId = ProductId??0, ProductTitle = product.Data?.Title })
                    });
                }
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
                if (product_image.UploadedFile == null || product_image.UploadedFile.Length == 0)
                {
                    if (!id.HasValue || id == 0)
                    {
                        ModelState.AddModelError("UploadedFile", "لطفا تصویر محصول را انتخاب نمایید");
                        html = await _renderService.ToStringAsync("_product_imageForm", product_image);
                        return new JsonResult(new { isValid = false, html = html });
                    }
                    else
                    {
                        ModelState.Remove("UploadedFile");
                    }
                }

                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    if (product_image.UploadedFile != null && product_image.UploadedFile.Length > 0)
                    {
                        var imageUrl = await _renderService.UploadImage(product_image.UploadedFile);
                        if (imageUrl == null)
                        {
                            _logger.LogError("product_image OnPostCreateOrEditAsync: " + imageUrl, product_image);

                            ModelState.AddModelError("UploadedFile", "امکان ثبت تصویر بنر وجود ندارد. خواهشمند است مجددا تلاش نمایید");
                            html = await _renderService.ToStringAsync("_product_imageForm", product_image);
                            return new JsonResult(new { isValid = false, html = html });
                        }

                        product_image.ImageUrl = imageUrl;
                    }
                    //var Api = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("EShopSettings")["StaicsServer"]
                    //    + "Statistics/UploadImage" ;
                    //string filePath = Path.Combine(contentRootPath, "images", new Int64().ToString(), product_image.UploadedFile.ContentType);

                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await product_image.UploadedFile.CopyToAsync(stream);
                    //}

                    product_image.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    if (!id.HasValue || id == 0)
                    {
                        await _product_imageRepository.AddAsync(product_image);
                    }
                    else
                    {
                        await _product_imageRepository.UpdateAsync(product_image);
                    }
                    html = await _renderService.ToStringAsync("_product_imageForm", new Product_ImageViewModel());
                    return await GetProduct_Images();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_product_imageForm", product_image);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("product_image OnPostCreateOrEditAsync: " + ex.Message, product_image);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64? ProductId, Int64? Product_OptionId, Int64 id)
        {
            try
            {
                await _product_imageRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Image OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetProduct_Images(ProductId, Product_OptionId);
        }

        private async Task<JsonResult> GetProduct_Images(Int64? pId, Int64? oId)
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _product_imageRepository.GetPaginatedResult(pId, oId);

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
