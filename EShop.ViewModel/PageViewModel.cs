using EShop.Utilities;

namespace EShop.ViewModel
{
    public class PageViewModel: BaseViewModel
    { 
		public string Title { get; set; }
		public string? ThemeAndOtherOptions { get; set; }
		public int? CategoryId { get; set; }
		public Guid? BrandId { get; set; }
		public bool? Confirmed { get; set; }
	}
}