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

namespace EShop.AdminPanel.Pages.Supplier.Supplier_Contract
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly ISupplier_ContractRepository _supplier_contractRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , ISupplier_ContractRepository supplier_contractRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _supplier_contractRepository = supplier_contractRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<Supplier_ContractViewModel>();
            try
            {
                var list = await _supplier_contractRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Contract OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_Supplier_ContractList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<Supplier_ContractViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_Supplier_ContractForm",
                        ViewData = new ViewDataDictionary<Supplier_ContractViewModel>(ViewData, new Supplier_ContractViewModel())
                    };
                else
                {
                    var supplier_contract = await _supplier_contractRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_Supplier_ContractForm",
                        ViewData = new ViewDataDictionary<Supplier_ContractViewModel>(ViewData, supplier_contract.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Contract OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_Supplier_ContractForm",
                ViewData = new ViewDataDictionary<Supplier_ContractViewModel>(ViewData, new Supplier_ContractViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Supplier_ContractForm", new Supplier_ContractViewModel()) });
                else
                {
                    var thisSupplier_Contract = await _supplier_contractRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Supplier_ContractForm", thisSupplier_Contract) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Contract OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, Supplier_ContractViewModel supplier_contract)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    supplier_contract.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _supplier_contractRepository.AddAsync(supplier_contract);
                    }
                    else
                    {
                        await _supplier_contractRepository.UpdateAsync(supplier_contract);
                    }
                    return await GetSupplier_Contracts();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_Supplier_ContractForm", supplier_contract);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Contract OnPostCreateOrEditAsync: " + ex.Message, supplier_contract);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _supplier_contractRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Contract OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetSupplier_Contracts();
        }

        private async Task<JsonResult> GetSupplier_Contracts()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _supplier_contractRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_Supplier_ContractList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Supplier_Contract GetSupplier_Contracts: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
