using EShop.Utilities;

namespace EShop.ViewModel
{
    public class OptionValueViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public long? ImageId { get; set; }
		public string? Color { get; set; }
		public string ImageTitle { get; set; }
	}
}