using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Item_Feature_DetailsViewModel: BaseViewModel
    { 
		public Guid? Item_FeatureId { get; set; }
		public string? Title { get; set; }
		public string? Image { get; set; }
		public string? details { get; set; }
		public bool? Confirmed { get; set; }
		public string Page_Item_FeatureTitle { get; set; }
	}
}