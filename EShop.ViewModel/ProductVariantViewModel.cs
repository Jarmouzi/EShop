using EShop.Utilities;

namespace EShop.ViewModel
{
    public class ProductVariantViewModel: BaseViewModel
    { 
		public long ProductId { get; set; }
		public long? SupplierId { get; set; }
		public bool? AvailableForSale { get; set; }
		public long? Price { get; set; }
		public bool? Confirmed { get; set; }
		public string SupplierTitle { get; set; }
		public string ProductTitle { get; set; }
	}
}