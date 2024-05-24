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

namespace EShop.AdminPanel.Pages.BasicInfo.Item_Feature_Detail
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IItem_Feature_DetailRepository _item_feature_detailRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IItem_Feature_DetailRepository item_feature_detailRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _item_feature_detailRepository = item_feature_detailRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<Item_Feature_DetailViewModel>();
            try
            {
                var list = await _item_feature_detailRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Detail OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_Item_Feature_DetailList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<Item_Feature_DetailViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new PartialViewResult
                    {
                        ViewName = "_Item_Feature_DetailForm",
                        ViewData = new ViewDataDictionary<Item_Feature_DetailViewModel>(ViewData, new Item_Feature_DetailViewModel())
                    };
                else
                {
                    var item_feature_detail = await _item_feature_detailRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_Item_Feature_DetailForm",
                        ViewData = new ViewDataDictionary<Item_Feature_DetailViewModel>(ViewData, item_feature_detail.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Detail OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_Item_Feature_DetailForm",
                ViewData = new ViewDataDictionary<Item_Feature_DetailViewModel>(ViewData, new Item_Feature_DetailViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Item_Feature_DetailForm", new Item_Feature_DetailViewModel()) });
                else
                {
                    var thisItem_Feature_Detail = await _item_feature_detailRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Item_Feature_DetailForm", thisItem_Feature_Detail) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Detail OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, Item_Feature_DetailViewModel item_feature_detail)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    item_feature_detail.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue)
                    {
                        await _item_feature_detailRepository.AddAsync(item_feature_detail);
                    }
                    else
                    {
                        await _item_feature_detailRepository.UpdateAsync(item_feature_detail);
                    }
                    return await GetItem_Feature_Details();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_Item_Feature_DetailForm", item_feature_detail);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Detail OnPostCreateOrEditAsync: " + ex.Message, item_feature_detail);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _item_feature_detailRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Detail OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetItem_Feature_Details();
        }

        private async Task<JsonResult> GetItem_Feature_Details()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _item_feature_detailRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_Item_Feature_DetailList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Item_Feature_Detail GetItem_Feature_Details: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
