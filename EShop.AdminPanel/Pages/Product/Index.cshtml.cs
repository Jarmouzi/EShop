using EShop.AdminPanel.Services;
using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.LogService.Repository;
using EShop.Model;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;

namespace EShop.AdminPanel.Pages.Product
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProductRepository productRepository
            , ICategoryRepository categoryRepository
            , IBrandRepository brandRepository
            , IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(Int64? categoryId = null, Int64? brandId = null, string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<ProductViewModel>();
            try
            {
                var list = await _productRepository.GetPaginatedResult(categoryId, brandId, title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Product OnGetViewAllPartial: " + ex.Message, [categoryId, title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_ProductList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<ProductViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                var result = await _categoryRepository.GetGroupedChildren();

                var bResult = await _brandRepository.GetAllAsync();

                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_ProductForm",
                        ViewData = new ViewDataDictionary<ProductViewModel>(ViewData, new ProductViewModel
                        {
                            Categories = new SelectList(result.Data, "Id", "Title", null, "ParentTitle"),
                            Brands = new SelectList(bResult.Data, "Id", "Title", null)
                        })
                    };
                else
                {
                    var product = await _productRepository.GetByIdAsync(id.Value);
                    product.Data.Categories = new SelectList(result.Data, "Id", "Title", product.Data.CategoryId, "ParentTitle");
                    product.Data.Brands = new SelectList(bResult.Data, "Id", "Title", product.Data.BrandId);

                    return new PartialViewResult
                    {
                        ViewName = "_ProductForm",
                        ViewData = new ViewDataDictionary<ProductViewModel>(ViewData, product.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Product OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_ProductForm",
                ViewData = new ViewDataDictionary<ProductViewModel>(ViewData, new ProductViewModel())
            };
        }
        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ProductForm", new ProductViewModel()) });
                else
                {
                    var thisProduct = await _productRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ProductForm", thisProduct) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Product OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, ProductViewModel product)
        {
            var html = "";
            try
            {
                if (ModelState.IsValid)
                {
                    product.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _productRepository.AddAsync(product);
                    }
                    else
                    {
                        await _productRepository.UpdateAsync(product);
                    }
                    return await GetProducts();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_ProductForm", product);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Product OnPostCreateOrEditAsync: " + ex.Message, product);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _productRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Product OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetProducts();
        }

        private async Task<JsonResult> GetProducts()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _productRepository.GetPaginatedResult(null, null, null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_ProductList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product GetProducts: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
