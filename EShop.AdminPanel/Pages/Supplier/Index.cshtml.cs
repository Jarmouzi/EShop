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

namespace EShop.AdminPanel.Pages.Supplier
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , ISupplierRepository supplierRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _supplierRepository = supplierRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<SupplierViewModel>();
            try
            {
                var list = await _supplierRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_SupplierList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<SupplierViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_SupplierForm",
                        ViewData = new ViewDataDictionary<SupplierViewModel>(ViewData, new SupplierViewModel())
                    };
                else
                {
                    var supplier = await _supplierRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_SupplierForm",
                        ViewData = new ViewDataDictionary<SupplierViewModel>(ViewData, supplier.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_SupplierForm",
                ViewData = new ViewDataDictionary<SupplierViewModel>(ViewData, new SupplierViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_SupplierForm", new SupplierViewModel()) });
                else
                {
                    var thisSupplier = await _supplierRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_SupplierForm", thisSupplier) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, SupplierViewModel supplier)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    supplier.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _supplierRepository.AddAsync(supplier);
                    }
                    else
                    {
                        await _supplierRepository.UpdateAsync(supplier);
                    }
                    return await GetSuppliers();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_SupplierForm", supplier);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier OnPostCreateOrEditAsync: " + ex.Message, supplier);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _supplierRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetSuppliers();
        }

        private async Task<JsonResult> GetSuppliers()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _supplierRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_SupplierList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier GetSuppliers: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
