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

namespace EShop.AdminPanel.Pages.BasicInfo.ProductVariant_Option
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProductVariant_OptionRepository _productvariant_optionRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProductVariant_OptionRepository productvariant_optionRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _productvariant_optionRepository = productvariant_optionRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<ProductVariant_OptionViewModel>();
            try
            {
                var list = await _productvariant_optionRepository.GetPaginatedResult(title, take, skip);

                result = list;
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant_Option OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_ProductVariant_OptionList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<ProductVariant_OptionViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_ProductVariant_OptionForm",
                        ViewData = new ViewDataDictionary<ProductVariant_OptionViewModel>(ViewData, new ProductVariant_OptionViewModel())
                    };
                else
                {
                    var productvariant_option = await _productvariant_optionRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_ProductVariant_OptionForm",
                        ViewData = new ViewDataDictionary<ProductVariant_OptionViewModel>(ViewData, productvariant_option)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant_Option OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_ProductVariant_OptionForm",
                ViewData = new ViewDataDictionary<ProductVariant_OptionViewModel>(ViewData, new ProductVariant_OptionViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ProductVariant_OptionForm", new ProductVariant_OptionViewModel()) });
                else
                {
                    var thisProductVariant_Option = await _productvariant_optionRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ProductVariant_OptionForm", thisProductVariant_Option) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant_Option OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, ProductVariant_OptionViewModel productvariant_option)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    productvariant_option.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _productvariant_optionRepository.AddAsync(productvariant_option);
                    }
                    else
                    {
                        await _productvariant_optionRepository.UpdateAsync(productvariant_option);
                    }
                    return await GetProductVariant_Options();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_ProductVariant_OptionForm", productvariant_option);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant_Option OnPostCreateOrEditAsync: " + ex.Message, productvariant_option);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _productvariant_optionRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant_Option OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetProductVariant_Options();
        }

        private async Task<JsonResult> GetProductVariant_Options()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _productvariant_optionRepository.GetPaginatedResult(null, 10, 0);

                

                html = await _renderService.ToStringAsync("_ProductVariant_OptionList", list);
            }
            catch (Exception ex)
            {
                _logger.LogError("ProductVariant_Option GetProductVariant_Options: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
