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

namespace EShop.AdminPanel.Pages.BasicInfo.PanelResource
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IPanelResourceRepository _panelresourceRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IPanelResourceRepository panelresourceRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _panelresourceRepository = panelresourceRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<PanelResourceViewModel>();
            try
            {
                var list = await _panelresourceRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("PanelResource OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_PanelResourceList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<PanelResourceViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_PanelResourceForm",
                        ViewData = new ViewDataDictionary<PanelResourceViewModel>(ViewData, new PanelResourceViewModel())
                    };
                else
                {
                    var panelresource = await _panelresourceRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_PanelResourceForm",
                        ViewData = new ViewDataDictionary<PanelResourceViewModel>(ViewData, panelresource.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PanelResource OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_PanelResourceForm",
                ViewData = new ViewDataDictionary<PanelResourceViewModel>(ViewData, new PanelResourceViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_PanelResourceForm", new PanelResourceViewModel()) });
                else
                {
                    var thisPanelResource = await _panelresourceRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_PanelResourceForm", thisPanelResource) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("PanelResource OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, PanelResourceViewModel panelresource)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    panelresource.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _panelresourceRepository.AddAsync(panelresource);
                    }
                    else
                    {
                        await _panelresourceRepository.UpdateAsync(panelresource);
                    }
                    return await GetPanelResources();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_PanelResourceForm", panelresource);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("PanelResource OnPostCreateOrEditAsync: " + ex.Message, panelresource);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _panelresourceRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("PanelResource OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetPanelResources();
        }

        private async Task<JsonResult> GetPanelResources()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _panelresourceRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_PanelResourceList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("PanelResource GetPanelResources: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
