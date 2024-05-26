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

namespace EShop.AdminPanel.Pages.BasicInfo.GroupType
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IGroupTypeRepository _grouptypeRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IGroupTypeRepository grouptypeRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _grouptypeRepository = grouptypeRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<GroupTypeViewModel>();
            try
            {
                var list = await _grouptypeRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("GroupType OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_GroupTypeList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<GroupTypeViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_GroupTypeForm",
                        ViewData = new ViewDataDictionary<GroupTypeViewModel>(ViewData, new GroupTypeViewModel())
                    };
                else
                {
                    var grouptype = await _grouptypeRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_GroupTypeForm",
                        ViewData = new ViewDataDictionary<GroupTypeViewModel>(ViewData, grouptype.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GroupType OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_GroupTypeForm",
                ViewData = new ViewDataDictionary<GroupTypeViewModel>(ViewData, new GroupTypeViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_GroupTypeForm", new GroupTypeViewModel()) });
                else
                {
                    var thisGroupType = await _grouptypeRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_GroupTypeForm", thisGroupType) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("GroupType OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, GroupTypeViewModel grouptype)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    grouptype.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Int64())
                    {
                        await _grouptypeRepository.AddAsync(grouptype);
                    }
                    else
                    {
                        await _grouptypeRepository.UpdateAsync(grouptype);
                    }
                    return await GetGroupTypes();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_GroupTypeForm", grouptype);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("GroupType OnPostCreateOrEditAsync: " + ex.Message, grouptype);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _grouptypeRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("GroupType OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetGroupTypes();
        }

        private async Task<JsonResult> GetGroupTypes()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _grouptypeRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_GroupTypeList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("GroupType GetGroupTypes: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
