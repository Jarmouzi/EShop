using EShop.AdminPanel.Services;
using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.LogService.Repository;
using EShop.Model;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EShop.AdminPanel.Pages.Product.Product_Feature
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProduct_FeatureRepository _product_featureRepository;
        private readonly IRazorRenderService _renderService;


        public IEnumerable<Product_FeatureViewModel> Product_Features { get; set; }

        [BindProperty]
        public ProductViewModel Product { get; set; }


        //[Parameter]
        //public int Id { get; set; }

        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProduct_FeatureRepository product_featureRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _product_featureRepository = product_featureRepository;
            _renderService = renderService;
        }
        public async Task OnGet()
        {
            //try
            //{
            //    Product_Features = _product_featureRepository.GetProductFeatures(Id).Result.Data;

            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("Product_Feature OnPostCreateOrEditAsync: " + ex.Message, Id);

            //    Product_Features = new List<Product_FeatureViewModel>();

            //}
        }
        //public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        //{
        //    var result = new PaginatedViewModel<Product_FeatureViewModel>();
        //    try
        //    {
        //        var list = await _product_featureRepository.GetPaginatedResult(title, take, skip);

        //        result = list.Data;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Product_Feature OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
        //    }

        //    return new PartialViewResult
        //    {
        //        ViewName = "_Product_FeatureList",
        //        ViewData = new ViewDataDictionary<PaginatedViewModel<Product_FeatureViewModel>>(ViewData, result)
        //    };
        //}
        public async Task<JsonResult> OnGetFormPartial(Int64 id)
        {
            try
            {
                Product_Features = _product_featureRepository.GetProductFeatures(id).Result.Data;
                //return new PartialViewResult
                //{
                //    ViewName = "_Product_FeatureForm",
                //    ViewData = new ViewDataDictionary<List<Product_FeatureViewModel>>(ViewData, Product_Features)
                //};
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_FeatureForm", Product_Features) });
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Feature OnPostCreateOrEditAsync: " + ex.Message, id);
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_FeatureForm", new List<Product_FeatureViewModel>()) });
            }
        }

        //public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        //{
        //    try
        //    {
        //        if (!id.HasValue || id == 0)
        //            return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_FeatureForm", new Product_FeatureViewModel()) });
        //        else
        //        {
        //            var thisProduct_Feature = await _product_featureRepository.GetByIdAsync(id.Value);
        //            return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_FeatureForm", thisProduct_Feature) });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Product_Feature OnPostCreateOrEditAsync: " + ex.Message, id);
        //    }
        //    return new JsonResult(new { isValid = true, html = "" });
        //}
        public async Task<JsonResult> OnPostCreateOrEditAsync([FromBody] List<SimpleProduct_FeatureViewModel> product_features)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                //if (ModelState.IsValid)
                //{
                //    product_feature.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                //    if (!id.HasValue || id == 0)
                //    {
                //        await _product_featureRepository.AddAsync(product_feature);
                //    }
                //    else
                //    {
                //        await _product_featureRepository.UpdateAsync(product_feature);
                //    }
                //    return await GetProduct_Features();
                //}
                //else
                //{
                //    html = await _renderService.ToStringAsync("_Product_FeatureForm", product_feature);
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Feature OnPostCreateOrEditAsync: " + ex.Message, product_features);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
    }
}
