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

namespace EShop.AdminPanel.Pages.BasicInfo.Product_Feature_Detail
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProduct_Feature_DetailRepository _product_feature_detailRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProduct_Feature_DetailRepository product_feature_detailRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _product_feature_detailRepository = product_feature_detailRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<Product_Feature_DetailViewModel>();
            try
            {
                var list = await _product_feature_detailRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Feature_Detail OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_Product_Feature_DetailList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<Product_Feature_DetailViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_Product_Feature_DetailForm",
                        ViewData = new ViewDataDictionary<Product_Feature_DetailViewModel>(ViewData, new Product_Feature_DetailViewModel())
                    };
                else
                {
                    var product_feature_detail = await _product_feature_detailRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_Product_Feature_DetailForm",
                        ViewData = new ViewDataDictionary<Product_Feature_DetailViewModel>(ViewData, product_feature_detail.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Feature_Detail OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_Product_Feature_DetailForm",
                ViewData = new ViewDataDictionary<Product_Feature_DetailViewModel>(ViewData, new Product_Feature_DetailViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_Feature_DetailForm", new Product_Feature_DetailViewModel()) });
                else
                {
                    var thisProduct_Feature_Detail = await _product_feature_detailRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_Feature_DetailForm", thisProduct_Feature_Detail) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Feature_Detail OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, Product_Feature_DetailViewModel product_feature_detail)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    product_feature_detail.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Int64())
                    {
                        await _product_feature_detailRepository.AddAsync(product_feature_detail);
                    }
                    else
                    {
                        await _product_feature_detailRepository.UpdateAsync(product_feature_detail);
                    }
                    return await GetProduct_Feature_Details();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_Product_Feature_DetailForm", product_feature_detail);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Feature_Detail OnPostCreateOrEditAsync: " + ex.Message, product_feature_detail);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _product_feature_detailRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Feature_Detail OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetProduct_Feature_Details();
        }

        private async Task<JsonResult> GetProduct_Feature_Details()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _product_feature_detailRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_Product_Feature_DetailList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Feature_Detail GetProduct_Feature_Details: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
