using EShop.Utilities;

namespace EShop.ViewModel
{
    public class FilterViewModel: BaseViewModel
    { 
		public Guid? FeatureId { get; set; }
		public Guid? CategoryId { get; set; }
		public string? ComponentName { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
		public string CategoryTitle { get; set; }
		public string FeatureTitle { get; set; }
	}
}