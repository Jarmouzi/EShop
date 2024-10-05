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

namespace EShop.AdminPanel.Pages.BasicInfo.Product_Option
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProduct_OptionRepository _product_optionRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProduct_OptionRepository product_optionRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _product_optionRepository = product_optionRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<Product_OptionViewModel>();
            try
            {
                var list = await _product_optionRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Option OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_Product_OptionList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<Product_OptionViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_Product_OptionForm",
                        ViewData = new ViewDataDictionary<Product_OptionViewModel>(ViewData, new Product_OptionViewModel())
                    };
                else
                {
                    var product_option = await _product_optionRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_Product_OptionForm",
                        ViewData = new ViewDataDictionary<Product_OptionViewModel>(ViewData, product_option.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Option OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_Product_OptionForm",
                ViewData = new ViewDataDictionary<Product_OptionViewModel>(ViewData, new Product_OptionViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_OptionForm", new Product_OptionViewModel()) });
                else
                {
                    var thisProduct_Option = await _product_optionRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_OptionForm", thisProduct_Option) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Option OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, Product_OptionViewModel product_option)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    product_option.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _product_optionRepository.AddAsync(product_option);
                    }
                    else
                    {
                        await _product_optionRepository.UpdateAsync(product_option);
                    }
                    return await GetProduct_Options();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_Product_OptionForm", product_option);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Option OnPostCreateOrEditAsync: " + ex.Message, product_option);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _product_optionRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Option OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetProduct_Options();
        }

        private async Task<JsonResult> GetProduct_Options()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _product_optionRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_Product_OptionList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Option GetProduct_Options: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
