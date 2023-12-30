using EShop.Utilities;

namespace EShop.ViewModel
{
    public class BrandViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public string? ThemeAndOtherOptions { get; set; }
		public bool? Confirmed { get; set; }
	}
}