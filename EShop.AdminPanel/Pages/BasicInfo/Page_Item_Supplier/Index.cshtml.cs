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

namespace EShop.AdminPanel.Pages.BasicInfo.Page_Item_Supplier
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IPage_Item_SupplierRepository _page_item_supplierRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IPage_Item_SupplierRepository page_item_supplierRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _page_item_supplierRepository = page_item_supplierRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<Page_Item_SupplierViewModel>();
            try
            {
                var list = await _page_item_supplierRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Supplier OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_Page_Item_SupplierList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<Page_Item_SupplierViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new PartialViewResult
                    {
                        ViewName = "_Page_Item_SupplierForm",
                        ViewData = new ViewDataDictionary<Page_Item_SupplierViewModel>(ViewData, new Page_Item_SupplierViewModel())
                    };
                else
                {
                    var page_item_supplier = await _page_item_supplierRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_Page_Item_SupplierForm",
                        ViewData = new ViewDataDictionary<Page_Item_SupplierViewModel>(ViewData, page_item_supplier.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Supplier OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_Page_Item_SupplierForm",
                ViewData = new ViewDataDictionary<Page_Item_SupplierViewModel>(ViewData, new Page_Item_SupplierViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Page_Item_SupplierForm", new Page_Item_SupplierViewModel()) });
                else
                {
                    var thisPage_Item_Supplier = await _page_item_supplierRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Page_Item_SupplierForm", thisPage_Item_Supplier) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Supplier OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, Page_Item_SupplierViewModel page_item_supplier)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    page_item_supplier.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue)
                    {
                        await _page_item_supplierRepository.AddAsync(page_item_supplier);
                    }
                    else
                    {
                        await _page_item_supplierRepository.UpdateAsync(page_item_supplier);
                    }
                    return await GetPage_Item_Suppliers();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_Page_Item_SupplierForm", page_item_supplier);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Supplier OnPostCreateOrEditAsync: " + ex.Message, page_item_supplier);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _page_item_supplierRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Supplier OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetPage_Item_Suppliers();
        }

        private async Task<JsonResult> GetPage_Item_Suppliers()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _page_item_supplierRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_Page_Item_SupplierList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Page_Item_Supplier GetPage_Item_Suppliers: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
