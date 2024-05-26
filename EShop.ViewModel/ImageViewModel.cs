using EShop.Utilities;

namespace EShop.ViewModel
{
    public class ImageViewModel: BaseViewModel
    { 
		public string? Url { get; set; }
		public string? AltText { get; set; }
		public int? Width { get; set; }
		public int? Height { get; set; }
		public bool? Confirmed { get; set; }
	}
}