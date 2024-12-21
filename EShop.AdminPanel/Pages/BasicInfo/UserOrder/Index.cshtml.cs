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

namespace EShop.AdminPanel.Pages.BasicInfo.UserOrder
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IUserOrderRepository _userorderRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IUserOrderRepository userorderRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _userorderRepository = userorderRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<UserOrderViewModel>();
            try
            {
                var list = await _userorderRepository.GetPaginatedResult(title, take, skip);

                result = list;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrder OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_UserOrderList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<UserOrderViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_UserOrderForm",
                        ViewData = new ViewDataDictionary<UserOrderViewModel>(ViewData, new UserOrderViewModel())
                    };
                else
                {
                    var userorder = await _userorderRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_UserOrderForm",
                        ViewData = new ViewDataDictionary<UserOrderViewModel>(ViewData, userorder)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrder OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_UserOrderForm",
                ViewData = new ViewDataDictionary<UserOrderViewModel>(ViewData, new UserOrderViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_UserOrderForm", new UserOrderViewModel()) });
                else
                {
                    var thisUserOrder = await _userorderRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_UserOrderForm", thisUserOrder) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrder OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, UserOrderViewModel userorder)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    userorder.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _userorderRepository.AddAsync(userorder);
                    }
                    else
                    {
                        await _userorderRepository.UpdateAsync(userorder);
                    }
                    return await GetUserOrders();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_UserOrderForm", userorder);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrder OnPostCreateOrEditAsync: " + ex.Message, userorder);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _userorderRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrder OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetUserOrders();
        }

        private async Task<JsonResult> GetUserOrders()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _userorderRepository.GetPaginatedResult(null, 10, 0);

                

                html = await _renderService.ToStringAsync("_UserOrderList", list);
            }
            catch (Exception ex)
            {
                _logger.LogError("UserOrder GetUserOrders: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
