using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Page_Item_SupplierViewModel: BaseViewModel
    { 
		public int? RemainedCount { get; set; }
		public Guid? SupplierId { get; set; }
		public long? Price { get; set; }
		public Int16? Discount { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
	}
}