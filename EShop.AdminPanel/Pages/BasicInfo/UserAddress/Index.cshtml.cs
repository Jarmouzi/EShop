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

namespace EShop.AdminPanel.Pages.BasicInfo.UserAddress
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IUserAddressRepository _useraddressRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IUserAddressRepository useraddressRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _useraddressRepository = useraddressRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<UserAddressViewModel>();
            try
            {
                var list = await _useraddressRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserAddress OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_UserAddressList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<UserAddressViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_UserAddressForm",
                        ViewData = new ViewDataDictionary<UserAddressViewModel>(ViewData, new UserAddressViewModel())
                    };
                else
                {
                    var useraddress = await _useraddressRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_UserAddressForm",
                        ViewData = new ViewDataDictionary<UserAddressViewModel>(ViewData, useraddress.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("UserAddress OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_UserAddressForm",
                ViewData = new ViewDataDictionary<UserAddressViewModel>(ViewData, new UserAddressViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_UserAddressForm", new UserAddressViewModel()) });
                else
                {
                    var thisUserAddress = await _useraddressRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_UserAddressForm", thisUserAddress) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserAddress OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, UserAddressViewModel useraddress)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    useraddress.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _useraddressRepository.AddAsync(useraddress);
                    }
                    else
                    {
                        await _useraddressRepository.UpdateAsync(useraddress);
                    }
                    return await GetUserAddresss();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_UserAddressForm", useraddress);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserAddress OnPostCreateOrEditAsync: " + ex.Message, useraddress);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _useraddressRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("UserAddress OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetUserAddresss();
        }

        private async Task<JsonResult> GetUserAddresss()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _useraddressRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_UserAddressList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("UserAddress GetUserAddresss: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
