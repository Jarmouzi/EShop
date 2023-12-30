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

namespace EShop.AdminPanel.Pages.BasicInfo.Stock
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IStockRepository _stockRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IStockRepository stockRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _stockRepository = stockRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<StockViewModel>();
            try
            {
                var list = await _stockRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Stock OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_StockList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<StockViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Guid id)
        {
            try
            {
                if (id == new Guid())
                    return new PartialViewResult
                    {
                        ViewName = "_StockForm",
                        ViewData = new ViewDataDictionary<StockViewModel>(ViewData, new StockViewModel())
                    };
                else
                {
                    var stock = await _stockRepository.GetByIdAsync(id);
                    return new PartialViewResult
                    {
                        ViewName = "_StockForm",
                        ViewData = new ViewDataDictionary<StockViewModel>(ViewData, stock.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Stock OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_StockForm",
                ViewData = new ViewDataDictionary<StockViewModel>(ViewData, new StockViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Guid id)
        {
            try
            {
                if (id == new Guid())
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_StockForm", new StockViewModel()) });
                else
                {
                    var thisStock = await _stockRepository.GetByIdAsync(id);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_StockForm", thisStock) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Stock OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Guid? id, StockViewModel stock)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    stock.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == null || id == new Guid())
                    {
                        await _stockRepository.AddAsync(stock);
                    }
                    else
                    {
                        await _stockRepository.UpdateAsync(stock);
                    }
                    return await GetStocks();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_StockForm", stock);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Stock OnPostCreateOrEditAsync: " + ex.Message, stock);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await _stockRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("Stock OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetStocks();
        }

        private async Task<JsonResult> GetStocks()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _stockRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_StockList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Stock GetStocks: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
