using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Supplier_ContractViewModel: BaseViewModel
    { 
		public Int64? SupplierId { get; set; }
		public Int64? CategoryId { get; set; }
		public Int64? SaleTypeId { get; set; }
		public int? Commission { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
		public string SupplierTitle { get; set; }
		public string CategoryTitle { get; set; }
		public string SaleTypeTitle { get; set; }
	}
}