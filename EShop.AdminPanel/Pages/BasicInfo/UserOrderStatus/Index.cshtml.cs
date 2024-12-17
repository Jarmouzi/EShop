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

namespace EShop.AdminPanel.Pages.BasicInfo.UserOrderStatus
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IUserOrderStatusRepository _userorderstatusRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IUserOrderStatusRepository userorderstatusRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _userorderstatusRepository = userorderstatusRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<UserOrderStatusViewModel>();
            try
            {
                var list = await _userorderstatusRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrderStatus OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_UserOrderStatusList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<UserOrderStatusViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_UserOrderStatusForm",
                        ViewData = new ViewDataDictionary<UserOrderStatusViewModel>(ViewData, new UserOrderStatusViewModel())
                    };
                else
                {
                    var userorderstatus = await _userorderstatusRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_UserOrderStatusForm",
                        ViewData = new ViewDataDictionary<UserOrderStatusViewModel>(ViewData, userorderstatus.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrderStatus OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_UserOrderStatusForm",
                ViewData = new ViewDataDictionary<UserOrderStatusViewModel>(ViewData, new UserOrderStatusViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_UserOrderStatusForm", new UserOrderStatusViewModel()) });
                else
                {
                    var thisUserOrderStatus = await _userorderstatusRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_UserOrderStatusForm", thisUserOrderStatus) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrderStatus OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, UserOrderStatusViewModel userorderstatus)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    userorderstatus.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _userorderstatusRepository.AddAsync(userorderstatus);
                    }
                    else
                    {
                        await _userorderstatusRepository.UpdateAsync(userorderstatus);
                    }
                    return await GetUserOrderStatuss();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_UserOrderStatusForm", userorderstatus);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrderStatus OnPostCreateOrEditAsync: " + ex.Message, userorderstatus);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _userorderstatusRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrderStatus OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetUserOrderStatuss();
        }

        private async Task<JsonResult> GetUserOrderStatuss()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _userorderstatusRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_UserOrderStatusList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrderStatus GetUserOrderStatuss: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
