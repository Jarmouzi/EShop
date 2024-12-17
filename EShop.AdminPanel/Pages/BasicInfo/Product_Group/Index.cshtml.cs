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

namespace EShop.AdminPanel.Pages.BasicInfo.Product_Group
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IProduct_GroupRepository _product_groupRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IProduct_GroupRepository product_groupRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _product_groupRepository = product_groupRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<Product_GroupViewModel>();
            try
            {
                var list = await _product_groupRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Group OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_Product_GroupList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<Product_GroupViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_Product_GroupForm",
                        ViewData = new ViewDataDictionary<Product_GroupViewModel>(ViewData, new Product_GroupViewModel())
                    };
                else
                {
                    var product_group = await _product_groupRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_Product_GroupForm",
                        ViewData = new ViewDataDictionary<Product_GroupViewModel>(ViewData, product_group.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Group OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_Product_GroupForm",
                ViewData = new ViewDataDictionary<Product_GroupViewModel>(ViewData, new Product_GroupViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_GroupForm", new Product_GroupViewModel()) });
                else
                {
                    var thisProduct_Group = await _product_groupRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Product_GroupForm", thisProduct_Group) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Group OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, Product_GroupViewModel product_group)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    product_group.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _product_groupRepository.AddAsync(product_group);
                    }
                    else
                    {
                        await _product_groupRepository.UpdateAsync(product_group);
                    }
                    return await GetProduct_Groups();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_Product_GroupForm", product_group);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Group OnPostCreateOrEditAsync: " + ex.Message, product_group);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _product_groupRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Group OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetProduct_Groups();
        }

        private async Task<JsonResult> GetProduct_Groups()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _product_groupRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_Product_GroupList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product_Group GetProduct_Groups: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
