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

namespace EShop.AdminPanel.Pages.BasicInfo.Page
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IPageRepository _pageRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IPageRepository pageRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _pageRepository = pageRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<PageViewModel>();
            try
            {
                var list = await _pageRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Page OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_PageList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<PageViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Guid id)
        {
            try
            {
                if (id == new Guid())
                    return new PartialViewResult
                    {
                        ViewName = "_PageForm",
                        ViewData = new ViewDataDictionary<PageViewModel>(ViewData, new PageViewModel())
                    };
                else
                {
                    var page = await _pageRepository.GetByIdAsync(id);
                    return new PartialViewResult
                    {
                        ViewName = "_PageForm",
                        ViewData = new ViewDataDictionary<PageViewModel>(ViewData, page.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Page OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_PageForm",
                ViewData = new ViewDataDictionary<PageViewModel>(ViewData, new PageViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Guid id)
        {
            try
            {
                if (id == new Guid())
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_PageForm", new PageViewModel()) });
                else
                {
                    var thisPage = await _pageRepository.GetByIdAsync(id);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_PageForm", thisPage) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Page OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Guid? id, PageViewModel page)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    page.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Guid())
                    {
                        await _pageRepository.AddAsync(page);
                    }
                    else
                    {
                        await _pageRepository.UpdateAsync(page);
                    }
                    return await GetPages();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_PageForm", page);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Page OnPostCreateOrEditAsync: " + ex.Message, page);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await _pageRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Page OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetPages();
        }

        private async Task<JsonResult> GetPages()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _pageRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_PageList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Page GetPages: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
