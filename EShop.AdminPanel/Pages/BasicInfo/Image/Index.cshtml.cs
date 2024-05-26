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

namespace EShop.AdminPanel.Pages.BasicInfo.Image
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IImageRepository _imageRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IImageRepository imageRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _imageRepository = imageRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<ImageViewModel>();
            try
            {
                var list = await _imageRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Image OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_ImageList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<ImageViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_ImageForm",
                        ViewData = new ViewDataDictionary<ImageViewModel>(ViewData, new ImageViewModel())
                    };
                else
                {
                    var image = await _imageRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_ImageForm",
                        ViewData = new ViewDataDictionary<ImageViewModel>(ViewData, image.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Image OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_ImageForm",
                ViewData = new ViewDataDictionary<ImageViewModel>(ViewData, new ImageViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ImageForm", new ImageViewModel()) });
                else
                {
                    var thisImage = await _imageRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_ImageForm", thisImage) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Image OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, ImageViewModel image)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    image.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Int64())
                    {
                        await _imageRepository.AddAsync(image);
                    }
                    else
                    {
                        await _imageRepository.UpdateAsync(image);
                    }
                    return await GetImages();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_ImageForm", image);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Image OnPostCreateOrEditAsync: " + ex.Message, image);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _imageRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Image OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetImages();
        }

        private async Task<JsonResult> GetImages()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _imageRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_ImageList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Image GetImages: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
