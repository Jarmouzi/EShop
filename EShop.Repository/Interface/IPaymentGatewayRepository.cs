using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface IPaymentGatewayRepository: IRepository<PaymentGateway, PaymentGatewayViewModel>
    {
        Task<Result<PaginatedViewModel<PaymentGatewayViewModel>>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
