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

namespace EShop.AdminPanel.Pages.BasicInfo.State
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IStateRepository _stateRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IStateRepository stateRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _stateRepository = stateRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<StateViewModel>();
            try
            {
                var list = await _stateRepository.GetPaginatedResult(title, take, skip);

                result = list;
            }
            catch (Exception ex)
            {
                _logger.LogError("State OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_StateList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<StateViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_StateForm",
                        ViewData = new ViewDataDictionary<StateViewModel>(ViewData, new StateViewModel())
                    };
                else
                {
                    var state = await _stateRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_StateForm",
                        ViewData = new ViewDataDictionary<StateViewModel>(ViewData, state)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("State OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_StateForm",
                ViewData = new ViewDataDictionary<StateViewModel>(ViewData, new StateViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_StateForm", new StateViewModel()) });
                else
                {
                    var thisState = await _stateRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_StateForm", thisState) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("State OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, StateViewModel state)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    state.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _stateRepository.AddAsync(state);
                    }
                    else
                    {
                        await _stateRepository.UpdateAsync(state);
                    }
                    return await GetStates();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_StateForm", state);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("State OnPostCreateOrEditAsync: " + ex.Message, state);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _stateRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("State OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetStates();
        }

        private async Task<JsonResult> GetStates()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _stateRepository.GetPaginatedResult(null, 10, 0);

                

                html = await _renderService.ToStringAsync("_StateList", list);
            }
            catch (Exception ex)
            {
                _logger.LogError("State GetStates: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
