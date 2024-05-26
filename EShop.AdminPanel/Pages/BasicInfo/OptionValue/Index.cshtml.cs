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

namespace EShop.AdminPanel.Pages.BasicInfo.OptionValue
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IOptionValueRepository _optionvalueRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IOptionValueRepository optionvalueRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _optionvalueRepository = optionvalueRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<OptionValueViewModel>();
            try
            {
                var list = await _optionvalueRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("OptionValue OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_OptionValueList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<OptionValueViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_OptionValueForm",
                        ViewData = new ViewDataDictionary<OptionValueViewModel>(ViewData, new OptionValueViewModel())
                    };
                else
                {
                    var optionvalue = await _optionvalueRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_OptionValueForm",
                        ViewData = new ViewDataDictionary<OptionValueViewModel>(ViewData, optionvalue.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("OptionValue OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_OptionValueForm",
                ViewData = new ViewDataDictionary<OptionValueViewModel>(ViewData, new OptionValueViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_OptionValueForm", new OptionValueViewModel()) });
                else
                {
                    var thisOptionValue = await _optionvalueRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_OptionValueForm", thisOptionValue) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("OptionValue OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, OptionValueViewModel optionvalue)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    optionvalue.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Int64())
                    {
                        await _optionvalueRepository.AddAsync(optionvalue);
                    }
                    else
                    {
                        await _optionvalueRepository.UpdateAsync(optionvalue);
                    }
                    return await GetOptionValues();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_OptionValueForm", optionvalue);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("OptionValue OnPostCreateOrEditAsync: " + ex.Message, optionvalue);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _optionvalueRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("OptionValue OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetOptionValues();
        }

        private async Task<JsonResult> GetOptionValues()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _optionvalueRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_OptionValueList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("OptionValue GetOptionValues: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
