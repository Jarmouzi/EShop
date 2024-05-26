using EShop.Utilities;

namespace EShop.ViewModel
{
    public class CollectionViewModel: BaseViewModel
    { 
		public string? CN { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Search { get; set; }
		public int? Count { get; set; }
		public bool? IsMain { get; set; }
	}
}