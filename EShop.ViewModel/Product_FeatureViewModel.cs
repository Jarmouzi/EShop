using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Product_FeatureViewModel: BaseViewModel
    { 
		public Int64? featureId { get; set; }
		public string? Value { get; set; }
		public Int64? ItemId { get; set; }
		public bool? Active { get; set; }
		public bool? IsMain { get; set; }
		public string? ComponentName { get; set; }
		public int? DisplayOrder { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
		public string FeatureTitle { get; set; }
		public string ProductTitle { get; set; }
	}
}