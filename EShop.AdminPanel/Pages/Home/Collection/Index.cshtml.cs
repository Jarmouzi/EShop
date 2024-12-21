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

namespace EShop.AdminPanel.Pages.BasicInfo.Collection
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly ICollectionRepository _collectionRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , ICollectionRepository collectionRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _collectionRepository = collectionRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<CollectionViewModel>();
            try
            {
                var list = await _collectionRepository.GetPaginatedResult(title, take, skip);

                result = list;
            }
            catch (Exception ex)
            {
                _logger.LogError("Collection OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_CollectionList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<CollectionViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_CollectionForm",
                        ViewData = new ViewDataDictionary<CollectionViewModel>(ViewData, new CollectionViewModel())
                    };
                else
                {
                    var collection = await _collectionRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_CollectionForm",
                        ViewData = new ViewDataDictionary<CollectionViewModel>(ViewData, collection)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Collection OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_CollectionForm",
                ViewData = new ViewDataDictionary<CollectionViewModel>(ViewData, new CollectionViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CollectionForm", new CollectionViewModel()) });
                else
                {
                    var thisCollection = await _collectionRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CollectionForm", thisCollection) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Collection OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, CollectionViewModel collection)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    collection.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Int64())
                    {
                        await _collectionRepository.AddAsync(collection);
                    }
                    else
                    {
                        await _collectionRepository.UpdateAsync(collection);
                    }
                    return await GetCollections();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_CollectionForm", collection);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Collection OnPostCreateOrEditAsync: " + ex.Message, collection);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _collectionRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Collection OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetCollections();
        }

        private async Task<JsonResult> GetCollections()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _collectionRepository.GetPaginatedResult(null, 10, 0);

                

                html = await _renderService.ToStringAsync("_CollectionList", list);
            }
            catch (Exception ex)
            {
                _logger.LogError("Collection GetCollections: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
