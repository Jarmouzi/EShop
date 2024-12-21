using EShop.Model;
using EShop.ViewModel;

namespace EShop.Repository.Interface
{
    public interface ISupplier_ContractRepository: IRepository<Supplier_Contract, Supplier_ContractViewModel>
    {
        Task<PaginatedViewModel<Supplier_ContractViewModel>> GetPaginatedResult(string? title = null, int take = 10, int skip = 0);
    }
}
