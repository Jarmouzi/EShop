using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Product_Feature_DetailViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public string? Detail { get; set; }
		public string? Image { get; set; }
		public long? Product_FeatureId { get; set; }
		public bool? Confirmed { get; set; }
		public string Product_FeatureTitle { get; set; }
	}
}