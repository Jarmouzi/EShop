using EShop.Utilities;

namespace EShop.ViewModel
{
    public class BannerViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public string? Image { get; set; }
		public string? Path { get; set; }
		public bool? Confirmed { get; set; }
	}
}