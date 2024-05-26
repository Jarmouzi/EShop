using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Supplier_BrandViewModel: BaseViewModel
    { 
		public long? SupplierId { get; set; }
		public long? BrandId { get; set; }
		public bool? Confirmed { get; set; }
		public string SupplierTitle { get; set; }
		public string BrandTitle { get; set; }
	}
}