using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Product_FeatureViewModel: BaseViewModel
    { 
		public Guid? featureId { get; set; }
		public string? Value { get; set; }
		public Guid? ItemId { get; set; }
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