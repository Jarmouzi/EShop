using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Supplier_ContractViewModel: BaseViewModel
    { 
		public Guid? SupplierId { get; set; }
		public Guid? CategoryId { get; set; }
		public Guid? SaleTypeId { get; set; }
		public int? Commission { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
		public string SupplierTitle { get; set; }
		public string CategoryTitle { get; set; }
		public string SaleTypeTitle { get; set; }
	}
}