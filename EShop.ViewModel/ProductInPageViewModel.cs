using EShop.Utilities;

namespace EShop.ViewModel
{
    public class ProductInPageViewModel: BaseViewModel
    { 
		public Guid? BrandId { get; set; }
		public Guid? ItemId { get; set; }
		public Guid? Item_FeatureId1 { get; set; }
		public Guid? Item_FeatureId2 { get; set; }
		public Guid? Item_FeatureId3 { get; set; }
		public Guid? SupplierId { get; set; }
		public Guid? Price { get; set; }
		public Guid? DealId { get; set; }
		public int? Discount { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
		public string ProductTitle { get; set; }
		public string Product_FeatureTitle1 { get; set; }
		public string Product_FeatureTitle2 { get; set; }
		public string Product_FeatureTitle3 { get; set; }
		public string SupplierTitle { get; set; }
	}
}