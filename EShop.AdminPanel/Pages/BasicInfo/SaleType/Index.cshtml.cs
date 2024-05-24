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

namespace EShop.AdminPanel.Pages.BasicInfo.SaleType
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly ISaleTypeRepository _saletypeRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , ISaleTypeRepository saletypeRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _saletypeRepository = saletypeRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<SaleTypeViewModel>();
            try
            {
                var list = await _saletypeRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("SaleType OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_SaleTypeList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<SaleTypeViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new PartialViewResult
                    {
                        ViewName = "_SaleTypeForm",
                        ViewData = new ViewDataDictionary<SaleTypeViewModel>(ViewData, new SaleTypeViewModel())
                    };
                else
                {
                    var saletype = await _saletypeRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_SaleTypeForm",
                        ViewData = new ViewDataDictionary<SaleTypeViewModel>(ViewData, saletype.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SaleType OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_SaleTypeForm",
                ViewData = new ViewDataDictionary<SaleTypeViewModel>(ViewData, new SaleTypeViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_SaleTypeForm", new SaleTypeViewModel()) });
                else
                {
                    var thisSaleType = await _saletypeRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_SaleTypeForm", thisSaleType) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("SaleType OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, SaleTypeViewModel saletype)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    saletype.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue)
                    {
                        await _saletypeRepository.AddAsync(saletype);
                    }
                    else
                    {
                        await _saletypeRepository.UpdateAsync(saletype);
                    }
                    return await GetSaleTypes();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_SaleTypeForm", saletype);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("SaleType OnPostCreateOrEditAsync: " + ex.Message, saletype);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _saletypeRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("SaleType OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetSaleTypes();
        }

        private async Task<JsonResult> GetSaleTypes()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _saletypeRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_SaleTypeList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaleType GetSaleTypes: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
