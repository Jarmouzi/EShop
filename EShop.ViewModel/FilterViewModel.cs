using EShop.Utilities;

namespace EShop.ViewModel
{
    public class FilterViewModel: BaseViewModel
    { 
		public Int64? FeatureId { get; set; }
		public Int64? CategoryId { get; set; }
		public string? ComponentName { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
		public string CategoryTitle { get; set; }
		public string FeatureTitle { get; set; }
	}
}