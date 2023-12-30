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

namespace EShop.AdminPanel.Pages.BasicInfo.Banner
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IBannerRepository _bannerRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IBannerRepository bannerRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _bannerRepository = bannerRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<BannerViewModel>();
            try
            {
                var list = await _bannerRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Banner OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_BannerList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<BannerViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Guid id)
        {
            try
            {
                if (id == new Guid())
                    return new PartialViewResult
                    {
                        ViewName = "_BannerForm",
                        ViewData = new ViewDataDictionary<BannerViewModel>(ViewData, new BannerViewModel())
                    };
                else
                {
                    var banner = await _bannerRepository.GetByIdAsync(id);
                    return new PartialViewResult
                    {
                        ViewName = "_BannerForm",
                        ViewData = new ViewDataDictionary<BannerViewModel>(ViewData, banner.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Banner OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_BannerForm",
                ViewData = new ViewDataDictionary<BannerViewModel>(ViewData, new BannerViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Guid id)
        {
            try
            {
                if (id == new Guid())
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_BannerForm", new BannerViewModel()) });
                else
                {
                    var thisBanner = await _bannerRepository.GetByIdAsync(id);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_BannerForm", thisBanner) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Banner OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Guid? id, BannerViewModel banner)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    banner.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Guid())
                    {
                        await _bannerRepository.AddAsync(banner);
                    }
                    else
                    {
                        await _bannerRepository.UpdateAsync(banner);
                    }
                    return await GetBanners();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_BannerForm", banner);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Banner OnPostCreateOrEditAsync: " + ex.Message, banner);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await _bannerRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Banner OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetBanners();
        }

        private async Task<JsonResult> GetBanners()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _bannerRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_BannerList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Banner GetBanners: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
