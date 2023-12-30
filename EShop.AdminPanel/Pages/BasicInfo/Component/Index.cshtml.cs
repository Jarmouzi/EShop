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

namespace EShop.AdminPanel.Pages.BasicInfo.Component
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IComponentRepository _componentRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IComponentRepository componentRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _componentRepository = componentRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<ComponentViewModel>();
            try
            {
                var list = await _componentRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Component OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_ComponentList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<ComponentViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Guid id)
        {
            try
            {
                if (id == new Guid())
                    return new PartialViewResult
                    {
                        ViewName = "_ComponentForm",
                        ViewData = new ViewDataDictionary<ComponentViewModel>(ViewData, new ComponentViewModel())
                    };
                else
                {
                    var component = await _componentRepository.GetByIdAsync(id);
                    return new PartialViewResult
                    {
                        ViewName = "_ComponentForm",
                        ViewData = new ViewDataDictionary<ComponentViewModel>(ViewData, component.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Component OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_ComponentForm",
                ViewData = new ViewDataDictionary<ComponentViewModel>(ViewData, new ComponentViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Guid id)
        {
            try
            {
                if (id == new Guid())
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ComponentForm", new ComponentViewModel()) });
                else
                {
                    var thisComponent = await _componentRepository.GetByIdAsync(id);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ComponentForm", thisComponent) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Component OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Guid? id, ComponentViewModel component)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    component.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Guid())
                    {
                        await _componentRepository.AddAsync(component);
                    }
                    else
                    {
                        await _componentRepository.UpdateAsync(component);
                    }
                    return await GetComponents();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_ComponentForm", component);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Component OnPostCreateOrEditAsync: " + ex.Message, component);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await _componentRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Component OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetComponents();
        }

        private async Task<JsonResult> GetComponents()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _componentRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_ComponentList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Component GetComponents: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
