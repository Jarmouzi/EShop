using EShop.Utilities;

namespace EShop.ViewModel
{
    public class ProductVariant_OptionViewModel: BaseViewModel
    { 
		public long? ProductVariantId { get; set; }
		public long? OptionId { get; set; }
		public long? OptionValueId { get; set; }
		public string ProductVariantTitle { get; set; }
		public string OptionTitle { get; set; }
		public string OptionValueTitle { get; set; }
	}
}