using EShop.AdminPanel.Services;
using EShop.IdentityService.Helper;
using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.LogService.Repository;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EShop.AdminPanel.Pages.Banner
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IBannerRepository _bannerRepository;
        private readonly IRazorRenderService _renderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IBannerRepository bannerRepository
            , IRazorRenderService renderService
            , IWebHostEnvironment webHostEnvironment
            , IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _dbLog = dbLog;
            _bannerRepository = bannerRepository;
            _renderService = renderService;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<BannerViewModel>();
            try
            {
                var list = await _bannerRepository.GetPaginatedResult(title, 10, 0);

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
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new PartialViewResult
                    {
                        ViewName = "_BannerForm",
                        ViewData = new ViewDataDictionary<BannerViewModel>(ViewData, new BannerViewModel())
                    };
                else
                {
                    var banner = await _bannerRepository.GetByIdAsync(id.Value);
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
        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_BannerForm", new BannerViewModel()) });
                else
                {
                    var thisBanner = await _bannerRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_BannerForm", thisBanner) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Banner OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, BannerViewModel banner)
        {
            var html = "";
            try
            {
                if (banner.UploadedFile == null || banner.UploadedFile.Length == 0)
                {
                    ModelState.AddModelError("UploadedFile", "لطفا تصویر بنر را انتخاب نمایید");
                    html = await _renderService.ToStringAsync("_BannerForm", banner);
                    return new JsonResult(new { isValid = false, html = html });
                }

                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    var imageUrl = await _renderService.UploadImage(banner.UploadedFile);
                    if (imageUrl == null || imageUrl.Contains("error"))
                    {
                        _logger.LogError("Banner OnPostCreateOrEditAsync: " + imageUrl, banner);

                        ModelState.AddModelError("UploadedFile", "امکان ثبت تصویر بنر وجود ندارد. خواهشمند است مجددا تلاش نمایید");
                        html = await _renderService.ToStringAsync("_BannerForm", banner);
                        return new JsonResult(new { isValid = false, html = html });
                    }

                    banner.Image = imageUrl;

                    //var Api = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("EShopSettings")["StaicsServer"]
                    //    + "Statistics/UploadImage" ;
                    //string filePath = Path.Combine(contentRootPath, "images", new Guid().ToString(), banner.UploadedFile.ContentType);

                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await banner.UploadedFile.CopyToAsync(stream);
                    //}

                    banner.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    if (!id.HasValue)
                    {
                        await _bannerRepository.AddAsync(banner);
                    }
                    else
                    {
                        await _bannerRepository.UpdateAsync(banner);
                    }
                    html = await _renderService.ToStringAsync("_BannerForm", new BannerViewModel());
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
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
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
