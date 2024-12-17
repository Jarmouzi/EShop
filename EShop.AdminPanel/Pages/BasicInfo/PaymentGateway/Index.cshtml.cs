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

namespace EShop.AdminPanel.Pages.BasicInfo.PaymentGateway
{
    [AuthorizePage]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
        private readonly IPaymentGatewayRepository _paymentgatewayRepository;
        private readonly IRazorRenderService _renderService;
        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IPaymentGatewayRepository paymentgatewayRepository,
            IRazorRenderService renderService)
        {
            _logger = logger;
            _dbLog = dbLog;
            _paymentgatewayRepository = paymentgatewayRepository;
            _renderService = renderService;
        }
        public void OnGet()
        {
        }
        public async Task<PartialViewResult> OnGetViewAllPartial(string? title = null, int take = 10, int skip = 0)
        {
            var result = new PaginatedViewModel<PaymentGatewayViewModel>();
            try
            {
                var list = await _paymentgatewayRepository.GetPaginatedResult(title, take, skip);

                result = list.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError("PaymentGateway OnGetViewAllPartial: " + ex.Message, [title, take, skip]);
            }

            return new PartialViewResult
            {
                ViewName = "_PaymentGatewayList",
                ViewData = new ViewDataDictionary<PaginatedViewModel<PaymentGatewayViewModel>>(ViewData, result)
            };
        }
        public async Task<PartialViewResult> OnGetFormPartial(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new PartialViewResult
                    {
                        ViewName = "_PaymentGatewayForm",
                        ViewData = new ViewDataDictionary<PaymentGatewayViewModel>(ViewData, new PaymentGatewayViewModel())
                    };
                else
                {
                    var paymentgateway = await _paymentgatewayRepository.GetByIdAsync(id.Value);
                    return new PartialViewResult
                    {
                        ViewName = "_PaymentGatewayForm",
                        ViewData = new ViewDataDictionary<PaymentGatewayViewModel>(ViewData, paymentgateway.Data)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PaymentGateway OnPostCreateOrEditAsync: " + ex.Message, id);
            }

            return new PartialViewResult
            {
                ViewName = "_PaymentGatewayForm",
                ViewData = new ViewDataDictionary<PaymentGatewayViewModel>(ViewData, new PaymentGatewayViewModel())
            };
        }

        public async Task<JsonResult> OnGetCreateOrEditAsync(Int64? id)
        {
            try
            {
                if (!id.HasValue || id == 0)
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_PaymentGatewayForm", new PaymentGatewayViewModel()) });
                else
                {
                    var thisPaymentGateway = await _paymentgatewayRepository.GetByIdAsync(id.Value);
                    return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_PaymentGatewayForm", thisPaymentGateway) });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("PaymentGateway OnPostCreateOrEditAsync: " + ex.Message, id);
            }
            return new JsonResult(new { isValid = true, html = "" });
        }
        public async Task<JsonResult> OnPostCreateOrEditAsync(Int64? id, PaymentGatewayViewModel paymentgateway)
        {
            var html = "";
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    paymentgateway.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (!id.HasValue || id == 0)
                    {
                        await _paymentgatewayRepository.AddAsync(paymentgateway);
                    }
                    else
                    {
                        await _paymentgatewayRepository.UpdateAsync(paymentgateway);
                    }
                    return await GetPaymentGateways();
                }
                else
                {
                    html = await _renderService.ToStringAsync("_PaymentGatewayForm", paymentgateway);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("PaymentGateway OnPostCreateOrEditAsync: " + ex.Message, paymentgateway);
            }
            return new JsonResult(new { isValid = false, html = html });
        }
        public async Task<JsonResult> OnPostDeleteAsync(Int64 id)
        {
            try
            {
                await _paymentgatewayRepository.DeleteAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError("PaymentGateway OnPostDeleteAsync: " + ex.Message, id);
            }
            return await GetPaymentGateways();
        }

        private async Task<JsonResult> GetPaymentGateways()
        {
            var isValid = false;
            var html = "";
            try
            {
                var list = await _paymentgatewayRepository.GetPaginatedResult(null, 10, 0);

                isValid = list.Status == TS.Status.Success;

                html = await _renderService.ToStringAsync("_PaymentGatewayList", list.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("PaymentGateway GetPaymentGateways: " + ex.Message);
            }
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
