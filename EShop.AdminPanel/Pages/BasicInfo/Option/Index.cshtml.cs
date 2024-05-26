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

namespace EShop.AdminPanel.Pages.BasicInfo.Option
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IOptionRepository _optionRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IOptionRepository optionRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _optionRepository = optionRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<OptionViewModel>();
            try
            {
                var list = await _optionRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Option OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_OptionList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<OptionViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_OptionForm",
                        ViewData = new ViewDataDictionary<OptionViewModel>(ViewData, new OptionViewModel())
                    };
                else
                {
                    var option = await _optionRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_OptionForm",
                        ViewData = new ViewDataDictionary<OptionViewModel>(ViewData, option.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Option OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_OptionForm",
                ViewData = new ViewDataDictionary<OptionViewModel>(ViewData, new OptionViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_OptionForm", new OptionViewModel()) });
                else
                {
                    var thisOption = await _optionRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_OptionForm", thisOption) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Option OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, OptionViewModel option)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    option.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Int64())
                    {
                        await _optionRepository.AddAsync(option);
                    }
                    else
                    {
                        await _optionRepository.UpdateAsync(option);
                    }
                    return await GetOptions();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_OptionForm", option);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Option OnPostCreateOrEditAsync: " + ex.Message, option);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _optionRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Option OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetOptions();
        }

        private async Task<JsonResult> GetOptions()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _optionRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_OptionList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Option GetOptions: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
