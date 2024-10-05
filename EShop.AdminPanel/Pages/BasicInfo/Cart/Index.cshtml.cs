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

namespace EShop.AdminPanel.Pages.BasicInfo.Cart
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly ICartRepository _cartRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , ICartRepository cartRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _cartRepository = cartRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<CartViewModel>();
            try
            {
                var list = await _cartRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Cart OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_CartList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<CartViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_CartForm",
                        ViewData = new ViewDataDictionary<CartViewModel>(ViewData, new CartViewModel())
                    };
                else
                {
                    var cart = await _cartRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_CartForm",
                        ViewData = new ViewDataDictionary<CartViewModel>(ViewData, cart.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Cart OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_CartForm",
                ViewData = new ViewDataDictionary<CartViewModel>(ViewData, new CartViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CartForm", new CartViewModel()) });
                else
                {
                    var thisCart = await _cartRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CartForm", thisCart) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Cart OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, CartViewModel cart)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    cart.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _cartRepository.AddAsync(cart);
                    }
                    else
                    {
                        await _cartRepository.UpdateAsync(cart);
                    }
                    return await GetCarts();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_CartForm", cart);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Cart OnPostCreateOrEditAsync: " + ex.Message, cart);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _cartRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Cart OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetCarts();
        }

        private async Task<JsonResult> GetCarts()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _cartRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_CartList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cart GetCarts: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
