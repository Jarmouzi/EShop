using EShop.Utilities;

namespace EShop.ViewModel
{
    public class ProductInPageViewModel: BaseViewModel
    { 
		public Int64? BrandId { get; set; }
		public Int64? ItemId { get; set; }
		public Int64? Item_FeatureId1 { get; set; }
		public Int64? Item_FeatureId2 { get; set; }
		public Int64? Item_FeatureId3 { get; set; }
		public Int64? SupplierId { get; set; }
		public Int64? Price { get; set; }
		public Int64? DealId { get; set; }
		public int? Discount { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
		public string ProductTitle { get; set; }
		//public string Product_FeatureTitle1 { get; set; }
		//public string Product_FeatureTitle2 { get; set; }
		//public string Product_FeatureTitle3 { get; set; }
		public string SupplierTitle { get; set; }
	}
}