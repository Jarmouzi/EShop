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

namespace EShop.AdminPanel.Pages.BasicInfo.UserProfile
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IUserProfileRepository _userprofileRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IUserProfileRepository userprofileRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _userprofileRepository = userprofileRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<UserProfileViewModel>();
            try
            {
                var list = await _userprofileRepository.GetPaginatedResult(title, take, skip);

                result = list;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserProfile OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_UserProfileList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<UserProfileViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_UserProfileForm",
                        ViewData = new ViewDataDictionary<UserProfileViewModel>(ViewData, new UserProfileViewModel())
                    };
                else
                {
                    var userprofile = await _userprofileRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_UserProfileForm",
                        ViewData = new ViewDataDictionary<UserProfileViewModel>(ViewData, userprofile)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("UserProfile OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_UserProfileForm",
                ViewData = new ViewDataDictionary<UserProfileViewModel>(ViewData, new UserProfileViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_UserProfileForm", new UserProfileViewModel()) });
                else
                {
                    var thisUserProfile = await _userprofileRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_UserProfileForm", thisUserProfile) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserProfile OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, UserProfileViewModel userprofile)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    userprofile.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _userprofileRepository.AddAsync(userprofile);
                    }
                    else
                    {
                        await _userprofileRepository.UpdateAsync(userprofile);
                    }
                    return await GetUserProfiles();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_UserProfileForm", userprofile);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("UserProfile OnPostCreateOrEditAsync: " + ex.Message, userprofile);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _userprofileRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("UserProfile OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetUserProfiles();
        }

        private async Task<JsonResult> GetUserProfiles()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _userprofileRepository.GetPaginatedResult(null, 10, 0);

                

                html = await _renderService.ToStringAsync("_UserProfileList", list);
            }
            catch (Exception ex)
            {
                _logger.LogError("UserProfile GetUserProfiles: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
