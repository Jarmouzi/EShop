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

namespace EShop.AdminPanel.Pages.BasicInfo.Item_Feature_Details
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IItem_Feature_DetailsRepository _item_feature_detailsRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IItem_Feature_DetailsRepository item_feature_detailsRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _item_feature_detailsRepository = item_feature_detailsRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<Item_Feature_DetailsViewModel>();
            try
            {
                var list = await _item_feature_detailsRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Details OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_Item_Feature_DetailsList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<Item_Feature_DetailsViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new PartialViewResult
                    {
                        ViewName = "_Item_Feature_DetailsForm",
                        ViewData = new ViewDataDictionary<Item_Feature_DetailsViewModel>(ViewData, new Item_Feature_DetailsViewModel())
                    };
                else
                {
                    var item_feature_details = await _item_feature_detailsRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_Item_Feature_DetailsForm",
                        ViewData = new ViewDataDictionary<Item_Feature_DetailsViewModel>(ViewData, item_feature_details.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Details OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_Item_Feature_DetailsForm",
                ViewData = new ViewDataDictionary<Item_Feature_DetailsViewModel>(ViewData, new Item_Feature_DetailsViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Item_Feature_DetailsForm", new Item_Feature_DetailsViewModel()) });
                else
                {
                    var thisItem_Feature_Details = await _item_feature_detailsRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Item_Feature_DetailsForm", thisItem_Feature_Details) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Details OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, Item_Feature_DetailsViewModel item_feature_details)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    item_feature_details.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue)
                    {
                        await _item_feature_detailsRepository.AddAsync(item_feature_details);
                    }
                    else
                    {
                        await _item_feature_detailsRepository.UpdateAsync(item_feature_details);
                    }
                    return await GetItem_Feature_Detailss();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_Item_Feature_DetailsForm", item_feature_details);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Details OnPostCreateOrEditAsync: " + ex.Message, item_feature_details);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _item_feature_detailsRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Details OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetItem_Feature_Detailss();
        }

        private async Task<JsonResult> GetItem_Feature_Detailss()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _item_feature_detailsRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_Item_Feature_DetailsList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Details GetItem_Feature_Detailss: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
