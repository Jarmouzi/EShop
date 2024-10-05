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

namespace EShop.AdminPanel.Pages.BasicInfo.CartItem
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly ICartItemRepository _cartitemRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , ICartItemRepository cartitemRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _cartitemRepository = cartitemRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<CartItemViewModel>();
            try
            {
                var list = await _cartitemRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("CartItem OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_CartItemList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<CartItemViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_CartItemForm",
                        ViewData = new ViewDataDictionary<CartItemViewModel>(ViewData, new CartItemViewModel())
                    };
                else
                {
                    var cartitem = await _cartitemRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_CartItemForm",
                        ViewData = new ViewDataDictionary<CartItemViewModel>(ViewData, cartitem.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("CartItem OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_CartItemForm",
                ViewData = new ViewDataDictionary<CartItemViewModel>(ViewData, new CartItemViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CartItemForm", new CartItemViewModel()) });
                else
                {
                    var thisCartItem = await _cartitemRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CartItemForm", thisCartItem) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("CartItem OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, CartItemViewModel cartitem)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    cartitem.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _cartitemRepository.AddAsync(cartitem);
                    }
                    else
                    {
                        await _cartitemRepository.UpdateAsync(cartitem);
                    }
                    return await GetCartItems();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_CartItemForm", cartitem);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("CartItem OnPostCreateOrEditAsync: " + ex.Message, cartitem);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _cartitemRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("CartItem OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetCartItems();
        }

        private async Task<JsonResult> GetCartItems()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _cartitemRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_CartItemList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("CartItem GetCartItems: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
