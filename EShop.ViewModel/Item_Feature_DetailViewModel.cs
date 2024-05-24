using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Item_Feature_DetailViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public string? Detail { get; set; }
		public string? Image { get; set; }
		public Int64? Item_FeatureId { get; set; }
		public bool? Confirmed { get; set; }
		public string Product_FeatureTitle { get; set; }
	}
}