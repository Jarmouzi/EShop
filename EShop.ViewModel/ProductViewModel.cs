using EShop.Utilities;

namespace EShop.ViewModel
{
    public class ProductViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public Int64? CategoryId { get; set; }
		public Int64? BrandId { get; set; }
		public bool? Confirmed { get; set; }
		public string BrandTitle { get; set; }
		public string CategoryTitle { get; set; }
	}
}